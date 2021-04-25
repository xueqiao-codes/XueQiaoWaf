using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class DiscreteOrderItemKey : Tuple<long, string, HostingXQOrderType, ClientXQOrderTargetType, string>
    {
        public DiscreteOrderItemKey(long subAccountId, string orderId,
            HostingXQOrderType orderType, ClientXQOrderTargetType targetType, string targetKey)
            : base(subAccountId, orderId, orderType, targetType, targetKey)
        {
            this.SubAccountId = subAccountId;
            this.OrderId = orderId;
            this.OrderType = orderType;
            this.TargetType = targetType;
            this.TargetKey = targetKey;
        }

        /// <summary>
        /// 操作账户 id
        /// </summary>
        public long SubAccountId { get; private set; }

        // 订单 id
        public string OrderId { get; private set; }

        // 订单类型
        public HostingXQOrderType OrderType { get; private set; }

        // 订单的标的类型
        public ClientXQOrderTargetType TargetType { get; private set; }

        // 订单的标的 key。 合约标的类型为 contract id，组合标的类型为 compose id。
        public string TargetKey { get; private set; }
    }
}
