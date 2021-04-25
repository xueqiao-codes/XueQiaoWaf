using business_foundation_lib.xq_thriftlib_config;
using ContainerShell.Applications.ViewModels;
using lib.xqclient_base.logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FeedbackDialogCtrl : IController
    {
        private readonly FeedbackVM contentVM;
        private readonly ILoginDataService loginDataService;
        
        private readonly IMessageWindowService messageWindowService;
        private readonly IFileDialogService fileDialogService;

        private readonly DelegateCommand selectPictureCmd;
        private readonly DelegateCommand deletePictureCmd;
        private readonly DelegateCommand submitCmd;
        private readonly DelegateCommand cancelCmd;

        private IMessageWindow dialog;
        private bool isSubmitting;
        private string selectedPictureFilePath;

        [ImportingConstructor]
        public FeedbackDialogCtrl(
            FeedbackVM contentVM,
            ILoginDataService loginDataService,
            
            IMessageWindowService messageWindowService,
            IFileDialogService fileDialogService)
        {
            this.contentVM = contentVM;
            this.loginDataService = loginDataService;
            
            this.messageWindowService = messageWindowService;
            this.fileDialogService = fileDialogService;

            selectPictureCmd = new DelegateCommand(SelectPicture);
            deletePictureCmd = new DelegateCommand(DeletePicture);
            submitCmd = new DelegateCommand(DoSubmit, CanSubmit);
            cancelCmd = new DelegateCommand(CloseDialog, CanCloseDialog);
        }

        public object DialogOwner { get; set; }

        public void Initialize()
        {
            contentVM.SelectPictureCmd = selectPictureCmd;
            contentVM.DeletePictureCmd = deletePictureCmd;
            contentVM.SubmitCmd = submitCmd;
            contentVM.CancelCmd = cancelCmd;
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, true, "反馈和建议", contentVM.View);
            dialog.Closing += Dialog_Closing;

            dialog.ShowDialog();
        }
        
        public void Shutdown()
        {
            InternalCloseDialog();
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");
        }
        
        private void Dialog_Closing(object sender, CancelEventArgs e)
        {
            if (this.isSubmitting)
            {
                var result = messageWindowService.ShowQuestionDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, null, "正在提交中，确认要关闭吗？", false, "关闭", "取消");
                e.Cancel = (result != true);
            }
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FeedbackVM.ContractPersonName)
                || e.PropertyName == nameof(FeedbackVM.ContractInformation)
                || e.PropertyName == nameof(FeedbackVM.FeedbackContent))
            {
                submitCmd?.RaiseCanExecuteChanged();
                cancelCmd?.RaiseCanExecuteChanged();
            }
        }

        private void UpdateIsSubmitting(bool value)
        {
            this.isSubmitting = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                contentVM.IsSubmiting = value;
                submitCmd?.RaiseCanExecuteChanged();
                cancelCmd?.RaiseCanExecuteChanged();
            });
        }

        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                try
                {
                    dialog.Closing -= Dialog_Closing;
                    dialog.Close();
                }
                catch (Exception)
                { }
                finally
                {
                    dialog = null;
                }
            }
        }

        private void SelectPicture()
        {
            var pictureFileTypes = new FileType[] { new FileType("JPEG", ".jpg"), new FileType("JPEG", ".jpeg"), new FileType("PNG", ".png") };
            var dialogResult = fileDialogService.ShowOpenFileDialog(UIHelper.GetWindowOfUIElement(contentVM.View), pictureFileTypes, pictureFileTypes.First(), null);
            if (dialogResult.IsValid)
            {
                this.selectedPictureFilePath = dialogResult.FileName;
                var selectedPictureName = Path.GetFileName(this.selectedPictureFilePath);
                contentVM.SelectedPictureName = selectedPictureName;
            }
        }

        private void DeletePicture()
        {
            this.selectedPictureFilePath = null;
            contentVM.SelectedPictureName = null;
        }

        private bool CanSubmit()
        {
            return !isSubmitting 
                && !string.IsNullOrEmpty(contentVM.ContractPersonName?.Trim())
                && !string.IsNullOrEmpty(contentVM.ContractInformation?.Trim())
                && !string.IsNullOrEmpty(contentVM.FeedbackContent?.Trim());
        }

        private void DoSubmit()
        {
            var contractPersonName = contentVM.ContractPersonName?.Trim();
            var contractInformation = contentVM.ContractInformation?.Trim();
            var feedbackContent = contentVM.FeedbackContent?.Trim();
            var client = new EmailClient
            {
                mailFrom = XueQiaoConstants.FeedbackMailFromAddress,
                mailPwd = XueQiaoBusinessHelper.FF_DP(),
                mailToArray = new string[] { XueQiaoConstants.FeedbackMailToAddress },
                mailSubject = $"[雪橇反馈建议] from {contractPersonName}",
                host = XueQiaoConstants.FeedbackMailSendHost,
                isbodyHtml = true
            };
            
            var bodySb = new StringBuilder();
            bodySb.Append("<ui>");
            bodySb.Append($"<li>联系人：{contractPersonName}</li>");
            bodySb.Append($"<li>联系方式：{contractInformation}</li>");
            bodySb.Append($"<li>环境：{XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironment.ToString()}</li>");
            bodySb.Append($"<li><p>登录用户信息：{loginDataService?.ProxyLoginResp?.ToString()}</p></li>");
            bodySb.Append($"<li><p>反馈建议：{feedbackContent}</p></li>");
            client.mailBody = bodySb.ToString();

            // 图片附件
            var attachmentsNameStream = new List<Tuple<string,Stream>>();
            var pictureFilePath = this.selectedPictureFilePath;
            if (!string.IsNullOrEmpty(pictureFilePath))
            {
                GetFileNameReadStream(pictureFilePath, out string fileName, out Stream readStream);
                if (readStream != null)
                {
                    attachmentsNameStream.Add(new Tuple<string, Stream>(fileName, readStream));
                }
            }

            // 日志文件附件
            if (contentVM.IsUploadLogChecked)
            {
                var logFileDir = AppLog.LogFileDir;
                if (!string.IsNullOrEmpty(logFileDir))
                {
                    var infoLogFilePath = Path.Combine(logFileDir, AppLog.InfoLogFileName);
                    GetFileNameReadStream(infoLogFilePath, out string infoLogFileName, out Stream infoLogFileReadStream);
                    if (infoLogFileReadStream != null)
                    {
                        attachmentsNameStream.Add(new Tuple<string, Stream>(infoLogFileName, infoLogFileReadStream));
                    }

                    var errorLogFilePah = Path.Combine(logFileDir, AppLog.ErrorLogFileName);
                    GetFileNameReadStream(errorLogFilePah, out string errorLogFileName, out Stream errorLogFileReadStream);
                    if (errorLogFileReadStream != null)
                    {
                        attachmentsNameStream.Add(new Tuple<string, Stream>(errorLogFileName, errorLogFileReadStream));
                    }
                }
            }

            client.attachmentsNameStream = attachmentsNameStream.ToArray();

            UpdateIsSubmitting(true);
            Task.Run(() => 
            {
                var r = client.Send(out Exception err);

                // close all attachments' stream
                foreach (var attachNameStream in attachmentsNameStream)
                {
                    attachNameStream.Item2.Close();
                }

                if (!r && err != null)
                {
                    AppLog.Error("Failed to send feedback mail.", err);
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    UpdateIsSubmitting(false);

                    // 如果界面已关闭，则不进行往下逻辑
                    if (dialog == null) return;

                    var owner = UIHelper.GetWindowOfUIElement(contentVM.View);
                    if (r)
                    {
                        messageWindowService.ShowMessageDialog(owner, null, null, null, "谢谢您的反馈和建议");
                        InternalCloseDialog();
                    }
                    else
                    {
                        messageWindowService.ShowMessageDialog(owner, null, null, null, "抱歉，反馈和建议提交失败，请稍后再试！");
                    }
                });
            });
        }

        private bool CanCloseDialog()
        {
            return !isSubmitting;
        }

        private void CloseDialog()
        {
            InternalCloseDialog();
        }

        private static void GetFileNameReadStream(string filePath, out string fileName, out Stream readStream)
        {
            fileName = null;
            readStream = null;
            try
            {
                fileName = Path.GetFileName(filePath);
                // Warning:考虑文件已经是写锁住时，将 FileShare 设置为 ReadWrite，才能让锁住的写继续写。
                // By creating a FileStream object and setting FileShare.ReadWrite I am saying that I want to open the file in a mode that allows other files to Read and Write to/from the file while I have it opened.
                // Refer: https://stackoverflow.com/questions/6167136/how-to-copy-a-file-while-it-is-being-used-by-another-process
                // http://coding.infoconex.com/post/2009/04/21/How-do-I-open-a-file-that-is-in-use-in-C
                readStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception e)
            {
                AppLog.Error($"Failed to GetFileNameStream, file({filePath}).", e);
            }
        }

    }
}
