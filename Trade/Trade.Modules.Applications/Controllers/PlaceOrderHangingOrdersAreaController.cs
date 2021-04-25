using NativeModel.Trade;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class PlaceOrderHangingOrdersAreaController : IController
    {
        private readonly PlaceOrderHangingOrdersAreaVM areaViewModel;
        private readonly ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl;
        
        [ImportingConstructor]
        public PlaceOrderHangingOrdersAreaController(PlaceOrderHangingOrdersAreaVM areaViewModel,
            ISelectedOrdersOperateCommandsCtrl selectedOrdersOperateCommandsCtrl)
        {
            this.areaViewModel = areaViewModel;
            this.selectedOrdersOperateCommandsCtrl = selectedOrdersOperateCommandsCtrl;
        }

        /// <summary>
        /// 挂单区域视图
        /// </summary>
        public object HangingOrdersAreaView => areaViewModel.View;
        
        public void Initialize()
        {
            selectedOrdersOperateCommandsCtrl.WindowOwnerGetter = () => UIHelper.GetWindowOfUIElement(areaViewModel.View);
            selectedOrdersOperateCommandsCtrl.Initialize();

            areaViewModel.SelectedOrdersOptCommands = selectedOrdersOperateCommandsCtrl.SelectedOrdersOptCommands;
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            selectedOrdersOperateCommandsCtrl.Shutdown();
        }

        /// <summary>
        /// 更新界面显示 key
        /// </summary>
        /// <param name="componentPresentKey"></param>
        public void UpdateViewPresentKey(PlaceOrderComponentPresentKey componentPresentKey)
        {
            areaViewModel.UpdateViewPresentKey(componentPresentKey); 
        }
    }
}
