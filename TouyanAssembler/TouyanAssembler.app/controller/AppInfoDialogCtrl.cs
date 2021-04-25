using business_foundation_lib.xq_thriftlib_config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using TouyanAssembler.app.viewmodel;
using TouyanAssembler.BusinessResource.constant;
using TouyanAssembler.BusinessResource.helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace TouyanAssembler.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AppInfoDialogCtrl : IController
    {
        private readonly AppInfoVM contentVM;
        private readonly IMessageWindowService messageWindowService;
        
        private IMessageWindow dialog;

        [ImportingConstructor]
        public AppInfoDialogCtrl(AppInfoVM contentVM,
            IMessageWindowService messageWindowService)
        {
            this.contentVM = contentVM;
            this.messageWindowService = messageWindowService;
            
        }

        /// <summary>
        /// 窗口 owner
        /// </summary>
        public object DialogOwner { get; set; }

        public void Initialize()
        {
            contentVM.SelectedApiEnvironment = XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironment;
            
            var isDevelopOpenStr = BusinessHelper.GetApplicationRegistryKey(Constants.RegistryKey_IsDevelopOpen);
            var isDevelopOpen = ("true" == isDevelopOpenStr.ToString()?.ToLower());
            contentVM.ShowApiEnvironmentSelectBox = isDevelopOpen;

            var currentVersion = AppVersionHelper.GetApplicationVersion();
            contentVM.CurrentVersionStr = $"v{currentVersion.ToString()}";

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropertyChanged, "");
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, true, "软件信息", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropertyChanged, "");
        }

        private void ContentVMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppInfoVM.SelectedApiEnvironment))
            {
                XqThriftLibConfigurationManager.SharedInstance.ThriftHttpLibEnvironment = contentVM.SelectedApiEnvironment;
                return;
            }
        }

        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                try
                {
                    dialog.Close();
                }
                catch (Exception) { }
                finally
                {
                    dialog = null;
                }
            }
        }
    }
}
