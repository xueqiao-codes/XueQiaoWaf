using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.Controllers
{
    /// <summary>
    /// 用户组选择弹窗 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SelectCompanyGroupDialogController : IController
    {
        private readonly SelectCompanyGroupContentVM contentVM;
        private readonly IMessageWindowService messageWindowService;

        private readonly DelegateCommand confirmCmd;
        private IMessageWindow dialog;

        [ImportingConstructor]
        public SelectCompanyGroupDialogController(SelectCompanyGroupContentVM contentVM,
            IMessageWindowService messageWindowService)
        {
            this.contentVM = contentVM;
            this.messageWindowService = messageWindowService;

            confirmCmd = new DelegateCommand(ConfirmSelection, CanConfirmSelection);
        }

        public object DialogOwner { get; set; }

        /// <summary>
        /// 可选择的公司组列表
        /// </summary>
        public IEnumerable<ProxyCompanyGroup> AvailableCompanyGroups { get; set; }

        /// <summary>
        /// 选取结果
        /// </summary>
        public ProxyCompanyGroup SelectedGroup { get; private set; }

        public void Initialize()
        {
            if (AvailableCompanyGroups == null) throw new ArgumentNullException("AvailableCompanyGroups");

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");

            contentVM.ConfirmCmd = confirmCmd;
            contentVM.CompanyGroups.AddRange(AvailableCompanyGroups.ToArray());
            contentVM.SelectedCompanyGroup = contentVM.CompanyGroups.FirstOrDefault();
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, 
                true, "选择交易云服务", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectCompanyGroupContentVM.SelectedCompanyGroup))
            {
                confirmCmd?.RaiseCanExecuteChanged();
            }
        }

        private bool CanConfirmSelection()
        {
            return contentVM.SelectedCompanyGroup?.Status == HostingServiceStatus.WORKING;
        }

        private void ConfirmSelection()
        {
            var selectGroup = contentVM.SelectedCompanyGroup;
            if (selectGroup == null) return;

            this.SelectedGroup = selectGroup;
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
    }
}
