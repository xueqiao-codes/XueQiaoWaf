using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Converters;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 雪橇组合订单执行参数模板管理弹窗 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XQComposeOrderEPTManDialogCtrl : IController
    {
        private readonly XQComposeOrderEPTManVM contentViewModel;
        private readonly XQComposeOrderEPTService _XQComposeOrderEPTService;
        private readonly IMessageWindowService messageWindowService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IEventAggregator eventAggregator;
        
        private readonly DelegateCommand newTemplateCmd;
        private readonly DelegateCommand templateParamsSetDefaultCmd;
        private readonly DelegateCommand templateSaveEditCmd;
        private readonly DelegateCommand templateCancelEditCmd;
        private readonly DelegateCommand templateOpenEditCmd;
        private readonly DelegateCommand templateRemoveCmd;

        private IMessageWindow dialog;

        private static readonly XQComposeOrderExecParamsSendType2NameConverter orderExecParamsSendType2NameConverter = new XQComposeOrderExecParamsSendType2NameConverter();

        [ImportingConstructor]
        public XQComposeOrderEPTManDialogCtrl(
            XQComposeOrderEPTManVM contentViewModel,
            XQComposeOrderEPTService _XQComposeOrderEPTService,
            IMessageWindowService messageWindowService,
            Lazy<ILoginUserManageService> loginUserManageService,
            IEventAggregator eventAggregator)
        {
            this.contentViewModel = contentViewModel;
            this._XQComposeOrderEPTService = _XQComposeOrderEPTService;
            this.messageWindowService = messageWindowService;
            this.loginUserManageService = loginUserManageService;
            this.eventAggregator = eventAggregator;

            newTemplateCmd = new DelegateCommand(NewTemplate);
            templateParamsSetDefaultCmd = new DelegateCommand(TemplateParamsSetDefault, CanTemplateParamsSetDefault);
            templateSaveEditCmd = new DelegateCommand(TemplateSaveEdit, CanTemplateSaveEdit);
            templateCancelEditCmd = new DelegateCommand(TemplateCancelEdit, CanTemplateCancelEdit);
            templateOpenEditCmd = new DelegateCommand(TemplateOpenEdit, CanTemplateOpenEdit);
            templateRemoveCmd = new DelegateCommand(TemplateRemove, CanTemplateRemove);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        public void Initialize()
        {
            contentViewModel.NewTemplateCmd = newTemplateCmd;
            contentViewModel.TemplateParamsSetDefaultCmd = templateParamsSetDefaultCmd;
            contentViewModel.TemplateSaveEditCmd = templateSaveEditCmd;
            contentViewModel.TemplateCancelEditCmd = templateCancelEditCmd;
            contentViewModel.TemplateOpenEditCmd = templateOpenEditCmd;
            contentViewModel.TemplateRemoveCmd = templateRemoveCmd;
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");
            contentViewModel.ActivedTemplate = contentViewModel.EPTs?.FirstOrDefault();
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(800, 800), true, true,
                true, "触发执行设置", contentViewModel.View);
            dialog.Closing += Dialog_DialogClosing;
            dialog.ShowDialog();
        }

        private void Dialog_DialogClosing(object sender, CancelEventArgs e)
        {
            var editingEPTs = _XQComposeOrderEPTService.EPTs.Where(i => i.IsInEditMode).ToArray();
            if (editingEPTs.Any())
            {
                var dialogOwner = UIHelper.GetWindowOfUIElement(contentViewModel.View);
                if (dialogOwner != null)
                {
                    var isNotClose = messageWindowService.ShowQuestionDialog(dialogOwner, null, null, null,
                        "还有正在编辑的内容未保存，是否放弃编辑？", false, "继续编辑", "放弃");
                    e.Cancel = (isNotClose == true);
                    if (isNotClose != true)
                    {
                        foreach (var EPT in editingEPTs)
                            GiveUpEditEPT(EPT);
                    }
                }
            }
        }

        public void Shutdown()
        {
            if (dialog != null)
            {
                dialog.Closing -= Dialog_DialogClosing;
                dialog.Close();
                dialog = null;
            }
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            PropertyChangedEventManager.AddHandler(contentViewModel, ContentViewModelPropChanged, "");
        }

        private void ContentViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XQComposeOrderEPTManVM.ActivedTemplate))
            {
                if (contentViewModel.ActivedTemplate != null)
                {
                    PropertyChangedEventManager.RemoveHandler(contentViewModel.ActivedTemplate, ComposeOrderEPTPropChanged, "");
                    PropertyChangedEventManager.AddHandler(contentViewModel.ActivedTemplate, ComposeOrderEPTPropChanged, "");
                }

                templateParamsSetDefaultCmd?.RaiseCanExecuteChanged();
                templateSaveEditCmd?.RaiseCanExecuteChanged();
                templateCancelEditCmd?.RaiseCanExecuteChanged();
                templateOpenEditCmd?.RaiseCanExecuteChanged();
                templateRemoveCmd?.RaiseCanExecuteChanged();
                
            }
        }

        private void ComposeOrderEPTPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XQComposeOrderExecParamsTemplate.Name))
            {
                this.templateSaveEditCmd?.RaiseCanExecuteChanged();
            }
            else if (e.PropertyName == nameof(XQComposeOrderExecParamsTemplate.IsInEditMode))
            {
                this.templateParamsSetDefaultCmd?.RaiseCanExecuteChanged();
                this.templateSaveEditCmd?.RaiseCanExecuteChanged();
                this.templateCancelEditCmd?.RaiseCanExecuteChanged();
                this.templateOpenEditCmd?.RaiseCanExecuteChanged();
            }
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void NewTemplate(object param)
        {
            var tType = param as XQComposeOrderExecParamsSendType?;
            if (tType == null) return;

            var templateName = (orderExecParamsSendType2NameConverter.Convert(tType.Value, typeof(string), null, CultureInfo.CurrentCulture)
                as string) ?? "执行参数模板";
            var template = new XQComposeOrderExecParamsTemplate(UUIDHelper.CreateUUIDString(true), tType.Value)
            {
                Name = templateName,
                IsArchived = false,
                IsInEditMode = true,
                CreateTimestampMs = (long)DateHelper.NowUnixTimeSpan().TotalSeconds
            };
            template.ConfigParamsAsDefault();

            // 添加到模板列表中
            _XQComposeOrderEPTService.EPTs.Add(template);
            contentViewModel.ActivedTemplate = template;
        }

        private void TemplateParamsSetDefault()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return;

            activeTemplate.ConfigParamsAsDefault();
        }

        private bool CanTemplateParamsSetDefault()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return false;
            return activeTemplate.IsInEditMode;
        }

        private void TemplateSaveEdit()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return;
            if (CheckExistSameNameArchivedEPT(activeTemplate, true)) return;

            var valErrMsg = activeTemplate.Validate();
            if (!string.IsNullOrEmpty(valErrMsg))
            {
                var containerWin = UIHelper.GetWindowOfUIElement(contentViewModel.View);
                messageWindowService.ShowMessageDialog(containerWin, null, null, null, $"填写正确的信息才能保存.\n{valErrMsg}");
                return;
            }

            var archivedItem = _XQComposeOrderEPTService.ArchivedEPTs.FirstOrDefault(i => i.Key == activeTemplate.Key);
            if (archivedItem == null)
            {
                // 保存归档
                archivedItem = new XQComposeOrderExecParamsTemplate(activeTemplate.Key, activeTemplate.TemplateType)
                {
                    IsInEditMode = false,
                    IsArchived = true
                };
                _XQComposeOrderEPTService.ArchivedEPTs.Add(archivedItem);
            }

            var mediation = new ComposeOrderEPT();
            XQComposeOrderEPTHelper.ConfigCLREPTWithDM(mediation, activeTemplate);
            XQComposeOrderEPTHelper.ConfigDMEPTWithCLR(archivedItem, mediation);

            activeTemplate.IsInEditMode = false;
            activeTemplate.IsArchived = true;
        }

        private bool CanTemplateSaveEdit()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return false;

            if (string.IsNullOrEmpty(activeTemplate.Name)) return false;
            return true;
        }
        
        private void TemplateCancelEdit()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return;
            if (CheckExistSameNameArchivedEPT(activeTemplate, true)) return;
            GiveUpEditEPT(activeTemplate);
        }

        private bool CanTemplateCancelEdit()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return false;
            return activeTemplate.IsInEditMode;
        }

        private void TemplateOpenEdit()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return;
            activeTemplate.IsInEditMode = true;
        }

        private bool CanTemplateOpenEdit()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return false;
            return !activeTemplate.IsInEditMode;
        }

        private void TemplateRemove()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return;

            var containerWin = UIHelper.GetWindowOfUIElement(contentViewModel.View);
            if (true != messageWindowService.ShowQuestionDialog(containerWin, null, null, null, "确定删除模板吗？", false, "删除", "取消"))
                return;

            _XQComposeOrderEPTService.EPTs.Remove(activeTemplate);
            _XQComposeOrderEPTService.ArchivedEPTs.RemoveAll(i => i.Key == activeTemplate.Key);
            contentViewModel.ActivedTemplate = _XQComposeOrderEPTService.EPTs.FirstOrDefault();
        }

        private bool CanTemplateRemove()
        {
            var activeTemplate = contentViewModel.ActivedTemplate;
            if (activeTemplate == null) return false;
            return true;
        }

        private void GiveUpEditEPT(XQComposeOrderExecParamsTemplate EPT)
        {
            if (EPT == null) return;
            EPT.IsInEditMode = false;
            
            var archivedItem = _XQComposeOrderEPTService.ArchivedEPTs.FirstOrDefault(i => i.Key == EPT.Key);
            if (archivedItem != null)
            {
                // 存在该 key 的归档模板，则恢复到为归档数据
                var mediation = new ComposeOrderEPT();
                XQComposeOrderEPTHelper.ConfigCLREPTWithDM(mediation, archivedItem);
                XQComposeOrderEPTHelper.ConfigDMEPTWithCLR(EPT, mediation);
            }
            else
            {
                // 不存在该 key 归档模板，则删除该模板
                _XQComposeOrderEPTService.EPTs.Remove(EPT);
            }
        }

        private bool CheckExistSameNameArchivedEPT(XQComposeOrderExecParamsTemplate compareEPT, bool alertWhenExist)
        {
            if (_XQComposeOrderEPTService.ArchivedEPTs.Any(i => i.Key != compareEPT.Key && i.Name == compareEPT.Name))
            {
                if (alertWhenExist)
                {
                    var containerWin = UIHelper.GetWindowOfUIElement(contentViewModel.View);
                    messageWindowService.ShowMessageDialog(containerWin, null, null, null, "存在同名的模板，请修改名称！");
                }
                return true;
            }
            return false;
        }
    }
}
