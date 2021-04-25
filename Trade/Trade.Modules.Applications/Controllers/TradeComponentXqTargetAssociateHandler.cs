using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 交易组件的标的联动处理器抽象接口
    /// </summary>
    internal interface ITradeComponentXqTargetAssociateHandler
    {
        /// <summary>
        /// 处理雪橇标的联动
        /// </summary>
        /// <param name="handleDialogOwner">如果处理过程中需要弹窗的话，提供处理弹窗的 owner</param>
        /// <param name="handleDialogLocation">如果处理过程中需要弹窗的话，提供处理弹窗的显示位置</param>
        /// <param name="associateArgs">联动发起参数</param>
        void HandleXqTargetAssociate(
            object handleDialogOwner,
            Point? handleDialogLocation,
            TradeComponentXqTargetAssociateArgs associateArgs);

        /// <summary>
        /// 新增雪橇标的下单窗口
        /// </summary>
        /// <param name="handleDialogOwner">如果处理过程中需要弹窗的话，提供处理弹窗的 owner</param>
        /// <param name="handleDialogLocation">如果处理过程中需要弹窗的话，提供处理弹窗的显示位置</param>
        /// <param name="associateArgs">联动发起参数</param>
        void NewXqTargetPlaceOrderComponent(
            object handleDialogOwner,
            Point? handleDialogLocation, 
            TradeComponentXqTargetAssociateArgs associateArgs);
    }

    /// <summary>
    /// 交易组件的标的联动发起参数
    /// </summary>
    internal class TradeComponentXqTargetAssociateArgs
    {
        public TradeComponentXqTargetAssociateArgs(TabWorkspace sourceWorkspace,
            TradeComponent sourceComponent,
            ClientXQOrderTargetType targetType,
            string targetKey,
            IDictionary<string, object> customInfos = null)
        {
            this.SourceWorkspace = sourceWorkspace;
            this.SourceComponent = sourceComponent;
            this.TargetType = targetType;
            this.TargetKey = targetKey;
            this.CustomInfos = customInfos;
        }

        /// <summary>
        /// 联动发起源所在工作空间
        /// </summary>
        public readonly TabWorkspace SourceWorkspace;

        /// <summary>
        /// 联动发起源所在的交易组件
        /// </summary>
        public readonly TradeComponent SourceComponent;

        /// <summary>
        /// 标的类型
        /// </summary>
        public readonly ClientXQOrderTargetType TargetType;

        /// <summary>
        /// 标的 key
        /// </summary>
        public readonly string TargetKey;

        /// <summary>
        /// 自定义信息
        /// </summary>
        public readonly IDictionary<string, object> CustomInfos;
    }
}
