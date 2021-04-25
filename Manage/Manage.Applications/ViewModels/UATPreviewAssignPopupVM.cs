using Manage.Applications.Services;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.trade.hosting;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UATPreviewAssignPopupVM : ViewModel<IUATPreviewAssignPopupView>
    {
        private readonly ManageSubAccountItemsService manSubAccountItemsService;

        [ImportingConstructor]
        protected UATPreviewAssignPopupVM(IUATPreviewAssignPopupView view,
            ManageSubAccountItemsService manSubAccountItemsService) : base(view)
        {
            this.manSubAccountItemsService = manSubAccountItemsService;
            this.SubAccountItems = new SynchronizingCollection<HostingSubAccount, HostingSubAccount>(manSubAccountItemsService.AccountItems, i => i);
            this.SelectedSubAccountId = this.SubAccountItems.FirstOrDefault()?.SubAccountId;
        }

        public event EventHandler Closed
        {
            add { ViewCore.Closed += value; }
            remove { ViewCore.Closed -= value; }
        }

        public void ShowPopup(object targetElement)
        {
            ViewCore.ShowPopup(targetElement);
        }

        public void Close()
        {
            ViewCore.Close();
        }

        public SynchronizingCollection<HostingSubAccount, HostingSubAccount> SubAccountItems { get; private set; }

        private long? selectedSubAccountId;
        public long? SelectedSubAccountId
        {
            get { return selectedSubAccountId; }
            set { SetProperty(ref selectedSubAccountId, value); }
        }

        private bool isShowAssignQuantityInputBox;
        /// <summary>
        /// 是否显示分配数量数量框
        /// </summary>
        public bool IsShowAssignQuantityInputBox
        {
            get { return isShowAssignQuantityInputBox; }
            set { SetProperty(ref isShowAssignQuantityInputBox, value); }
        }

        private int assignQuantity;
        /// <summary>
        /// 分配数量
        /// </summary>
        public int AssignQuantity
        {
            get { return assignQuantity; }
            set { SetProperty(ref assignQuantity, value); }
        }

        private int maxAssignQuantity;
        /// <summary>
        /// 最大可分配数量
        /// </summary>
        public int MaxAssignQuantity
        {
            get { return maxAssignQuantity; }
            set { SetProperty(ref maxAssignQuantity, value); }
        }

        private ICommand confirmAssignCmd;
        public ICommand ConfirmAssignCmd
        {
            get { return confirmAssignCmd; }
            set { SetProperty(ref confirmAssignCmd, value); }
        }
    }
}
