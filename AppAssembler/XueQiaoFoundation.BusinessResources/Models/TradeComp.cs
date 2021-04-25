using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 交易组件
    /// </summary>
    public class TradeComp
    {
        public TradeComp()
        {
            SubscribeDataContainerComponentDetail = new SubscribeDataContainerComponentDetail();
            PlaceOrderContainerComponentDetail = new PlaceOrderContainerComponentDetail();
            AccountContainerComponentDetail = new AccountContainerComponentDetail();
        }

        // 位置 left
        public double Left { get; set; }

        // 位置 top
        public double Top { get; set; }

        // 大小 width
        public double Width { get; set; }

        // 大小 height
        public double Height { get; set; }

        /// <summary>
        /// 组件的 Zindex。组件叠放在一起时，ZIndex 大的显示在上层。
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// 交易组件的类型。参考 XueQiaoConstants 定义的`交易组件类型`
        /// </summary>
        public int ComponentType { get; set; }
        
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 组件描述标题
        /// </summary>
        public string ComponentDescTitle { get; set; }
        
        /// <summary>
        /// 订阅数据容器组件详情
        /// </summary>
        public SubscribeDataContainerComponentDetail SubscribeDataContainerComponentDetail { get; set; }

        /// <summary>
        /// 下单容器组件详情
        /// </summary>
        public PlaceOrderContainerComponentDetail PlaceOrderContainerComponentDetail { get; set; }

        /// <summary>
        /// 账号容器组件详情
        /// </summary>
        public AccountContainerComponentDetail AccountContainerComponentDetail { get; set; }
    }


    /// <summary>
    /// 订阅数据容器组件详情
    /// </summary>
    public class SubscribeDataContainerComponentDetail
    {
        /// <summary>
        /// 一起tab的组件类型
        /// </summary>
        public int[] TogatherTabedComponentTypes { get; set; }

        /// <summary>
        /// 合约列表组件详情
        /// </summary>
        public ContractListComponentDetail ContractListComponentDetail { get; set; }

        /// <summary>
        /// 组合列表组件详情
        /// </summary>
        public ComposeListComponentDetail ComposeListComponentDetail { get; set; }
    }

    /// <summary>
    /// 下单容器组件详情
    /// </summary>
    public class PlaceOrderContainerComponentDetail
    {
        /// <summary>
        /// 是否显示图表布局视图
        /// </summary>
        public bool IsShowChartLayout { get; set; }

        /// <summary>
        /// 是否显示下单布局视图
        /// </summary>
        public bool IsShowOrderLayout { get; set; }

        /// <summary>
        /// 附着的合约标的 id。存在且有效时表示标的为合约
        /// </summary>
        public string AttachContractId { get; set; }

        /// <summary>
        /// 附着的组合标的 id。存在且有效时表示标的为组合
        /// </summary>
        public string AttachComposeId { get; set; }
    }

    /// <summary>
    /// 账号容器组件详情
    /// </summary>
    public class AccountContainerComponentDetail
    {
        /// <summary>
        /// 一起tab的组件类型
        /// </summary>
        public int[] TogatherTabedComponentTypes { get; set; }

        /// <summary>
        /// 委托单列表组件详情
        /// </summary>
        public EntrustedOrderListComponentDetail EntrustedOrderListComponentDetail { get; set; }

        /// <summary>
        /// 预埋单列表组件详情
        /// </summary>
        public ParkedOrderListComponentDetail ParkedOrderListComponentDetail { get; set; }

        /// <summary>
        /// 条件单列表组件详情
        /// </summary>
        public ConditionOrderListComponentDetail ConditionOrderListComponentDetail { get; set; }

        /// <summary>
        /// 成交列表组件详情
        /// </summary>
        public TradeListComponentDetail TradeListComponentDetail { get; set; }

        /// <summary>
        /// 持仓列表组件详情
        /// </summary>
        public PositionListComponentDetail PositionListComponentDetail { get; set; }

        /// <summary>
        /// 持仓助手组件详情
        /// </summary>
        public PositionAssistantComponentDetail PositionAssistantComponentDetail { get; set; }

        /// <summary>
        /// 订单历史组件详情
        /// </summary>
        public OrderHistoryComponentDetail OrderHistoryComponentDetail { get; set; }

        /// <summary>
        /// 成交历史组件详情
        /// </summary>
        public TradeHistoryComponentDetail TradeHistoryComponentDetail { get; set; }

        /// <summary>
        /// 持仓历史分配组件详情
        /// </summary>
        public PositionAssignHistoryComponentDetail PositionAssignHistoryComponentDetail { get; set; }

        /// <summary>
        /// 资金列表组件详情
        /// </summary>
        public FundListComponentDetail FundListComponentDetail { get; set; }
    }

    /// <summary>
    /// 订阅合约列表组件详情
    /// </summary>
    public class ContractListComponentDetail
    {
        /// <summary>
        /// 选中的列表分组 key
        /// </summary>
        public string SelectedListGroupKey { get; set; }

        /// <summary>
        /// 列表显示的列
        /// </summary>
        public ListColumnInfo[] ContractListColumns { get; set; }
    }

    /// <summary>
    /// 订阅组合列表组件详情
    /// </summary>
    public class ComposeListComponentDetail
    {
        /// <summary>
        /// 选中的列表分组 key
        /// </summary>
        public string SelectedListGroupKey { get; set; }

        /// <summary>
        /// 列表显示的列
        /// </summary>
        public ListColumnInfo[] ComposeListColumns { get; set; }
    }

    /// <summary>
    /// 委托单列表组件详情
    /// </summary>
    public class EntrustedOrderListComponentDetail
    {
        /// <summary>
        /// 订单列表显示列
        /// </summary>
        public ListColumnInfo[] OrderListColumns { get; set; }
    }

    /// <summary>
    /// 预埋单组件详情
    /// </summary>
    public class ParkedOrderListComponentDetail
    {
        /// <summary>
        /// 订单列表显示列
        /// </summary>
        public ListColumnInfo[] OrderListColumns { get; set; }
    }

    /// <summary>
    /// 条件单组件详情
    /// </summary>
    public class ConditionOrderListComponentDetail
    {
        /// <summary>
        /// 订单列表显示列
        /// </summary>
        public ListColumnInfo[] OrderListColumns { get; set; }
    }

    /// <summary>
    /// 成交列表组件详情
    /// </summary>
    public class TradeListComponentDetail
    {
        /// <summary>
        /// 成交列表显示列
        /// </summary>
        public ListColumnInfo[] TradeListColumns { get; set; }
    }

    /// <summary>
    /// 持仓列表组件详情
    /// </summary>
    public class PositionListComponentDetail
    {
    }

    /// <summary>
    /// 持仓助手组件详情
    /// </summary>
    public class PositionAssistantComponentDetail
    {

    }

    /// <summary>
    /// 资金列表组件详情
    /// </summary>
    public class FundListComponentDetail
    {
    }

    /// <summary>
    /// 订单历史组件详情
    /// </summary>
    public class OrderHistoryComponentDetail
    {

    }

    /// <summary>
    /// 成交历史组件详情
    /// </summary>
    public class TradeHistoryComponentDetail
    {

    }

    /// <summary>
    /// 持仓历史分配组件详情
    /// </summary>
    public class PositionAssignHistoryComponentDetail
    {

    }
}
