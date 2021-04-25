using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    /// <summary>
    /// 交易组件联动常量
    /// </summary>
    public static class TradeComponentAssociateConstants
    {
        /// <summary>
        /// 组件联动参数：价格。其 value 类型为 <see cref="double"/>
        /// </summary>
        public const string ComponentAssociateArg_Price = "Price";

        /// <summary>
        /// 组件联动参数：下单类型。其value 类型为 <see cref="NativeModel.Trade.ClientPlaceOrderType"/>
        /// </summary>
        public const string ComponentAssociateArg_PlaceOrderType = "PlaceOrderType";

        /// <summary>
        /// 组件联动参数：是否显示图表视图。其value 类型为 <see cref="bool"/>
        /// </summary>
        public const string ComponentAssociateArg_IsShowChartLayout = "IsShowChartLayout";

        /// <summary>
        /// 组件联动参数：是否显示下单视图。其value 类型为 <see cref="bool"/>
        /// </summary>
        public const string ComponentAssociateArg_IsShowPlaceOrderLayout = "IsShowPlaceOrderLayout";
    }
}
