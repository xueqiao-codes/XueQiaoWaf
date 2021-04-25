using AppAssembler.Interfaces.Applications;
using ContainerShell.Applications.DataModels;
using ContainerShell.Applications.ViewModels;
using lib.xqclient_base.logger;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqPreferenceSettingDialogCtrl : IController
    {
        private readonly XqPreferenceSettingVM contentVM;
        private readonly ILoginDataService loginDataService;
        private readonly IMessageWindowService messageWindowService;
        private readonly IFileDialogService fileDialogService;
        private readonly IAppAssemblerService appAssemblerService; 

        private readonly DelegateCommand selectFileSoundCmd;
        private readonly DelegateCommand saveCmd;
        private readonly DelegateCommand cancelCmd;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public XqPreferenceSettingDialogCtrl(
            XqPreferenceSettingVM contentVM,
            ILoginDataService loginDataService,
            
            IMessageWindowService messageWindowService,
            IFileDialogService fileDialogService,
            IAppAssemblerService appAssemblerService)
        {
            this.contentVM = contentVM;
            this.loginDataService = loginDataService;
            this.messageWindowService = messageWindowService;
            this.fileDialogService = fileDialogService;
            this.appAssemblerService = appAssemblerService;

            saveCmd = new DelegateCommand(SavePref);
            cancelCmd = new DelegateCommand(CloseDialog);
            selectFileSoundCmd = new DelegateCommand(SelectFileSound);
        }

        public object DialogOwner { get; set; }

        public void Initialize()
        {
            // 支持语言：中文简体
            contentVM.AppSupportLanguageList.Add(XqAppLanguages.CN);

            // 支持主题：浅色、深色
            contentVM.AppSupportThemeList.Add(XqAppThemeType.LIGHT);
            contentVM.AppSupportThemeList.Add(XqAppThemeType.DARK);

            contentVM.SaveCmd = saveCmd;
            contentVM.CancelCmd = cancelCmd;
            contentVM.SelectFileSoundCmd = selectFileSoundCmd;
            contentVM.AppPref = XqAppPreference_ModelHelper.ToXqAppPreferenceDM(appAssemblerService.PreferenceManager.Config);
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, true, "系统设置", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
        }

        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }

        private void SavePref()
        {
            var originTheme = appAssemblerService.PreferenceManager.Config.AppTheme;
            var originLanguage = appAssemblerService.PreferenceManager.Config.Language;

            var prefObj = XqAppPreference_ModelHelper.ToXqAppPreference(contentVM.AppPref);
            appAssemblerService.PreferenceManager.SaveConfig(prefObj, out Exception _exception);
            if (_exception != null)
            {
                AppLog.Error("Failed to save config.", _exception);
            }

            if (originTheme != prefObj.AppTheme)
            {
                appAssemblerService.ApplyTheme(prefObj.AppTheme);
            }

            if (originLanguage != prefObj.Language)
            {
                appAssemblerService.ApplyLanguage(contentVM.AppPref.Language);
            }

            InternalCloseDialog();
        }

        private void CloseDialog()
        {
            InternalCloseDialog();
        }

        private void SelectFileSound(object obj)
        {
            var fileSoundType = obj as SelectFileSoundType?;
            if (fileSoundType == null) return;

            var fileTypes = new FileType[] { new FileType(".wav 格式文件", ".wav") };
            var dialogResult = fileDialogService.ShowOpenFileDialog(UIHelper.GetWindowOfUIElement(contentVM.View), fileTypes, fileTypes.First(), null);
            if (dialogResult.IsValid)
            {
                var filePath = dialogResult.FileName;
                switch (fileSoundType)
                {
                    case SelectFileSoundType.OrderTraded:
                        contentVM.AppPref.OrderTradedAudioFileName = filePath;
                        break;
                    case SelectFileSoundType.OrderTriggered:
                        contentVM.AppPref.OrderTriggeredAudioFileName = filePath;
                        break;
                    case SelectFileSoundType.OrderErr:
                        contentVM.AppPref.OrderErrAudioFileName = filePath;
                        break;
                    case SelectFileSoundType.LameTraded:
                        contentVM.AppPref.LameTradedAudioFileName = filePath;
                        break;
                    case SelectFileSoundType.OrderAmbiguous:
                        contentVM.AppPref.OrderAmbiguousAudioFileName = filePath;
                        break;
                    case SelectFileSoundType.OrderOtherNotify:
                        contentVM.AppPref.OrderOtherNotifyAudioFileName = filePath;
                        break;
                }
            }
        }
    }
}
