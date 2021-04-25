using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Interface;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 预分配信息
    /// </summary>
    internal class UATPreviewAssignInfo : Tuple<long, int>
    {
        public UATPreviewAssignInfo(long selectedSubAccountId, int assignQuantity) : base(selectedSubAccountId, assignQuantity)
        {
            this.SelectedSubAccountId = selectedSubAccountId;
            this.AssignQuantity = assignQuantity;
        }

        public readonly long SelectedSubAccountId;
        public readonly int AssignQuantity;
    }

    /// <summary>
    /// 未分配成交预分配弹层 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class UATPreviewAssignPopupCtrl : IController
    {
        private readonly UATPreviewAssignPopupVM popupViewModel;
        private readonly DelegateCommand confirmAssignCmd;

        [ImportingConstructor]
        public UATPreviewAssignPopupCtrl(UATPreviewAssignPopupVM popupViewModel)
        {
            this.popupViewModel = popupViewModel;
            confirmAssignCmd = new DelegateCommand(ConfirmAssign, CanConfirmAssign);
        }

        /// <summary>
        /// 是否显示分配数量输入框
        /// </summary>
        public bool IsShowAssignQuantityInputBox { get; set; }

        /// <summary>
        /// 设置的最大可预分配数量
        /// </summary>
        public int MaxAssignQuantity { get; set; }

        /// <summary>
        /// 弹层位置目标
        /// </summary>
        public object PopupPalcementTarget { get; set; }

        /// <summary>
        /// 关闭回调。arg0:controller, arg1:预分配的信息
        /// </summary>
        public Action<UATPreviewAssignPopupCtrl, UATPreviewAssignInfo> PopupCloseHandler { get; set; }

        public void Initialize()
        {
            PropertyChangedEventManager.AddHandler(popupViewModel, PopupVMPropChanged, "");

            popupViewModel.Closed += PopupViewModel_Closed;
            popupViewModel.IsShowAssignQuantityInputBox = IsShowAssignQuantityInputBox;
            popupViewModel.AssignQuantity = 0;
            popupViewModel.MaxAssignQuantity = MaxAssignQuantity;
            popupViewModel.ConfirmAssignCmd = confirmAssignCmd;
        }

        public void Run()
        {
            popupViewModel.ShowPopup(PopupPalcementTarget);
        }

        public void Shutdown()
        {
            popupViewModel.Closed -= PopupViewModel_Closed;
            PropertyChangedEventManager.RemoveHandler(popupViewModel, PopupVMPropChanged, "");
            PopupCloseHandler = null;
        }
        
        private void PopupVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UATPreviewAssignPopupVM.SelectedSubAccountId)
                || e.PropertyName == nameof(UATPreviewAssignPopupVM.IsShowAssignQuantityInputBox)
                || e.PropertyName == nameof(UATPreviewAssignPopupVM.AssignQuantity))
            {
                confirmAssignCmd?.RaiseCanExecuteChanged();
            }
        }

        private void PopupViewModel_Closed(object sender, EventArgs e)
        {
            Shutdown();
        }

        private bool CanConfirmAssign()
        {
            if (popupViewModel.SelectedSubAccountId == null) return false;
            if (popupViewModel.IsShowAssignQuantityInputBox)
            {
                return popupViewModel.AssignQuantity > 0;
            }
            return true;
        }

        private void ConfirmAssign()
        {
            var selectedSubAccountId = popupViewModel.SelectedSubAccountId;
            var assignQuantity = popupViewModel.AssignQuantity;
            if (selectedSubAccountId == null) return;

            popupViewModel.Closed -= PopupViewModel_Closed;
            PopupCloseHandler?.Invoke(this, new UATPreviewAssignInfo(selectedSubAccountId.Value, assignQuantity));
            popupViewModel.Close();
        }
    }
}
