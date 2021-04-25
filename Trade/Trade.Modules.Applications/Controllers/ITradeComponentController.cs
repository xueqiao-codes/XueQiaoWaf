using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 交易组件控制器抽象接口
    /// </summary>
    internal interface ITradeComponentController : IController
    {
        /// <summary>
        /// 交易组件所属父工作区
        /// </summary>
        TabWorkspace ParentWorkspace { get; }

        /// <summary>
        /// 交易组件
        /// </summary>
        TradeComponent Component { get; }

        /// <summary>
        /// 组件 controller 关闭处理器
        /// </summary>
        Action<ITradeComponentController> CloseComponentHandler { get; }

        /// <summary>
        /// 组件间的雪橇标的联动处理器
        /// </summary>
        ITradeComponentXqTargetAssociateHandler XqTargetAssociateHandler { get; }

        /// <summary>
        /// 组件 data model
        /// </summary>
        DraggableComponentUIDM ComponentItemDataModel { get; }

        /// <summary>
        /// 在当前组件中联动雪橇标的
        /// </summary>
        /// <param name="associateArgs">联动发起参数</param>
        /// <returns>是否进行了联动</returns>
        bool OnAssociateXqTarget(TradeComponentXqTargetAssociateArgs associateArgs);
    }
}
