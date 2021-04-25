using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using xueqiao.trade.hosting.position.statis;
using xueqiao.trade.hosting.terminal.ao;

namespace NativeModel.Trade
{
    public static class Extensions
    {
        /// <summary>
        /// 转化为本地用户组合视图 INativeComposeGraphView
        /// </summary>
        /// <param name="hostingComposeViewDetail"></param>
        /// <returns></returns>
        public static NativeComposeViewDetail ToNativeComposeViewDetail(this HostingComposeViewDetail hostingComposeViewDetail)
        {
            if (hostingComposeViewDetail == null) return null;
            var tarViewDetail = new NativeComposeViewDetail
            {
                ComposeGraph = hostingComposeViewDetail.GraphDetail?.ToNativeComposeGraph(),
                UserComposeView = hostingComposeViewDetail.ViewDetail?.ToNativeComposeView()
            };
            return tarViewDetail;
        }

        /// <summary>
        /// 转化为本地 NativeComposeView
        /// </summary>
        /// <param name="hostingComposeView">用户组合视图</param>
        /// <returns></returns>
        public static NativeComposeView ToNativeComposeView(this HostingComposeView hostingComposeView)
        {
            if (hostingComposeView == null) return null;
            var userTarView = new NativeComposeView(hostingComposeView.ComposeGraphId, hostingComposeView.SubUserId);
            userTarView.AliasName = hostingComposeView.AliasName;
            userTarView.CreateTimestamp = hostingComposeView.CreateTimestamp;
            userTarView.SubscribeStatus = hostingComposeView.SubscribeStatus.ToClientComposeViewSubscribeStatus();
            userTarView.PrecisionNumber = hostingComposeView.PrecisionNumber;
            return userTarView;
        }

        /// <summary>
        /// 转化为本地 NativeComposeGraph
        /// </summary>
        /// <param name="hostingComposeGraph">组合图信息</param>
        public static NativeComposeGraph ToNativeComposeGraph(this HostingComposeGraph hostingComposeGraph)
        {
            if (hostingComposeGraph == null) return null;
            var tarItem = new NativeComposeGraph(hostingComposeGraph.ComposeGraphId);
            tarItem.CreateSubUserId = hostingComposeGraph.CreateSubUserId;
            tarItem.Formular = hostingComposeGraph.Formular;
            
            var legs = hostingComposeGraph.Legs?.Values
                .OrderBy(srcLeg => srcLeg.VariableName)
                .Select(srcLeg => new NativeComposeLeg(hostingComposeGraph.ComposeGraphId, srcLeg.SledContractId)
                {
                    VariableName = srcLeg.VariableName,
                    Quantity = srcLeg.Quantity,
                    TradeDirection = srcLeg.LegTradeDirection.ToClientTradeDirection(),
                    SledContractCode = srcLeg.SledContractCode,
                    SledCommodityId = srcLeg.SledCommodityId,
                    SledCommodityType = srcLeg.SledCommodityType,
                    SledCommodityCode = srcLeg.SledCommodityCode,
                    SledExchangeMic = srcLeg.SledExchangeMic
                }).ToArray();
            if (legs != null)
            {
                tarItem.Legs = new ObservableCollection<NativeComposeLeg>(legs);
            }

            tarItem.ComposeGraphKey = hostingComposeGraph.ComposeGraphKey;
            tarItem.CreateTimestamp = hostingComposeGraph.CreateTimestamp;
            return tarItem;
        }

        /// <summary>
        /// 转化为组合视图本地订阅状态类型
        /// </summary>
        /// <param name="hostingItem"></param>
        /// <returns></returns>
        public static ClientComposeViewSubscribeStatus ToClientComposeViewSubscribeStatus(this HostingComposeViewSubscribeStatus hostingItem)
        {
            if (Enum.TryParse(hostingItem.ToString(), out ClientComposeViewSubscribeStatus clientType))
            {
                return clientType;
            }
            return ClientComposeViewSubscribeStatus.UNSUBSCRIBED;
        }

        /// <summary>
        /// 转化为本地的交易方向
        /// </summary>
        /// <param name="xqTradeDir">类型为<see cref="HostingXQTradeDirection"/>的交易方向</param>
        /// <returns></returns>
        public static ClientTradeDirection ToClientTradeDirection(this HostingXQTradeDirection xqTradeDir)
        {
            switch (xqTradeDir)
            {
                case HostingXQTradeDirection.XQ_BUY:
                    return ClientTradeDirection.BUY;
                case HostingXQTradeDirection.XQ_SELL:
                    return ClientTradeDirection.SELL;
                default:
                    return ClientTradeDirection.BUY;
            }
        }
        
        /// <summary>
        /// 转化为本地的交易方向
        /// </summary>
        /// <param name="cmposeLegTradeDir">类型为<see cref="HostingComposeLegTradeDirection"/>的交易方向</param>
        /// <returns></returns>
        public static ClientTradeDirection ToClientTradeDirection(this HostingComposeLegTradeDirection cmposeLegTradeDir)
        {
            switch (cmposeLegTradeDir)
            {
                case HostingComposeLegTradeDirection.COMPOSE_LEG_BUY:
                    return ClientTradeDirection.BUY;
                case HostingComposeLegTradeDirection.COMPOSE_LEG_SELL:
                    return ClientTradeDirection.SELL;
                default:
                    return ClientTradeDirection.BUY;
            }
        }

        /// <summary>
        /// 转化为本地的交易方向
        /// </summary>
        /// <param name="cmposeLegTradeDir">类型为<see cref="StatDirection"/>的交易方向</param>
        /// <returns></returns>
        public static ClientTradeDirection ToClientTradeDirection(this StatDirection statDirection)
        {
            switch (statDirection)
            {
                case StatDirection.STAT_BUY:
                    return ClientTradeDirection.BUY;
                case StatDirection.STAT_SELL:
                    return ClientTradeDirection.SELL;
                default:
                    return ClientTradeDirection.BUY;
            }
        }

        /// <summary>
        /// 转化为 <see cref="StatDirection"/> 的方向
        /// </summary>
        /// <param name="clientTradeDirection">类型为<see cref="ClientTradeDirection"/>的交易方向</param>
        /// <returns></returns>
        public static StatDirection ToStatDirection(this ClientTradeDirection clientTradeDirection)
        {
            switch (clientTradeDirection)
            {
                case ClientTradeDirection.BUY:
                    return StatDirection.STAT_BUY;
                case ClientTradeDirection.SELL:
                    return StatDirection.STAT_SELL;
                default:
                    return StatDirection.STAT_BUY;
            }
        }

        public static ClientXQOrderTargetType ToClientXQOrderTargetType(this HostingXQTargetType hostingXQTargetType)
        {
            if (Enum.TryParse(hostingXQTargetType.ToString(), out ClientXQOrderTargetType clientType))
            {
                return clientType;
            }
            return ClientXQOrderTargetType.CONTRACT_TARGET;
        }

        public static HostingXQTargetType ToHostingXQTargetType(this ClientXQOrderTargetType clientXQOrderTargetType)
        {
            if (Enum.TryParse(clientXQOrderTargetType.ToString(), out HostingXQTargetType hostingType))
            {
                return hostingType;
            }
            return HostingXQTargetType.CONTRACT_TARGET;
        }

        public static ClientXQOrderState ToClientXQOrderState(this HostingXQOrderStateValue hostingXQOrderStateVal)
        {
            if (Enum.TryParse(hostingXQOrderStateVal.ToString(), out ClientXQOrderState cientState))
            {
                return cientState;
            }
            return ClientXQOrderState.ClientInaccurate_Unkown;
        }

        /// <summary>
        /// 将委托单的执行订单状态映射成本地订单状态
        /// </summary>
        /// <param name="execOrderStateValue"></param>
        /// <returns></returns>
        public static ClientXQOrderState? EntrustedOrderExecStateMap2ClientXQOrderState(this HostingExecOrderStateValue execOrderStateValue)
        {
            switch (execOrderStateValue)
            {
                case HostingExecOrderStateValue.ORDER_WAITING_VERIFY:
                case HostingExecOrderStateValue.ORDER_WAITING_SLED_SEND:
                case HostingExecOrderStateValue.ORDER_SLED_SEND_UNKOWN:
                case HostingExecOrderStateValue.ORDER_UPSIDE_RECEIVED:
                case HostingExecOrderStateValue.ORDER_UPSIDE_RESTING:
                case HostingExecOrderStateValue.ORDER_UPSIDE_PARTFINISHED:
                case HostingExecOrderStateValue.ORDER_SLED_ALLOC_REF_FINISHED:
                    return ClientXQOrderState.XQ_ORDER_RUNNING;

                case HostingExecOrderStateValue.ORDER_VERIFY_FAILED:
                case HostingExecOrderStateValue.ORDER_SLED_SEND_FAILED:
                case HostingExecOrderStateValue.ORDER_UPSIDE_REJECTED:
                case HostingExecOrderStateValue.ORDER_UPSIDE_DELETED:
                case HostingExecOrderStateValue.ORDER_EXPIRED:
                    return ClientXQOrderState.XQ_ORDER_CANCELLED;

                case HostingExecOrderStateValue.ORDER_UPSIDE_FINISHED:
                    return ClientXQOrderState.XQ_ORDER_FINISHED;

                case HostingExecOrderStateValue.ORDER_UPSIDE_RECEIVED_WAITING_REVOKE:
                case HostingExecOrderStateValue.ORDER_UPSIDE_RESTING_WAITING_REVOKE:
                case HostingExecOrderStateValue.ORDER_UPSIDE_PARTFINISHED_WAITING_REVOKE:
                case HostingExecOrderStateValue.ORDER_UPSIDE_RECEIVED_REVOKE_SEND_UNKOWN:
                case HostingExecOrderStateValue.ORDER_UPSIDE_RESTING_REVOKE_SEND_UNKOWN:
                case HostingExecOrderStateValue.ORDER_UPSIDE_PARTFINISHED_REVOKE_SEND_UNKOWN:
                case HostingExecOrderStateValue.ORDER_UPSIDE_REVOKE_RECEIVED:
                    return ClientXQOrderState.XQ_ORDER_CANCELLING;

                default:
                    return null;
            }
        }


        /// <summary>
        /// 将<see cref="xueqiao.trade.hosting.position.statis.DataSourceChannel"/>转换成<see cref="XqTargetPositionDataSourceChannel"/> 类型
        /// </summary>
        /// <param name="idlDataSourceChannel"></param>
        /// <returns></returns>
        public static XqTargetPositionDataSourceChannel? ToXqTargetPositionDataSourceChannel(this xueqiao.trade.hosting.position.statis.DataSourceChannel idlDataSourceChannel)
        {
            if (Enum.TryParse(idlDataSourceChannel.ToString(), out XqTargetPositionDataSourceChannel clientType))
            {
                return clientType;
            }
            return null;
        }

        /// <summary>
        /// 解析雪橇组合订单发单方式
        /// </summary>
        /// <param name="execParams"></param>
        /// <returns></returns>
        public static XQComposeOrderExecParamsSendType? ParseComposeOrderExecParamsSendType(this HostingXQComposeLimitOrderExecParams execParams)
        {
            if (execParams == null) return null;

            if (execParams.ExecType == HostingXQComposeLimitOrderExecType.PARALLEL_LEG)
                return XQComposeOrderExecParamsSendType.PARALLEL_LEG;
            else if (execParams.ExecType == HostingXQComposeLimitOrderExecType.LEG_BY_LEG
                && execParams.ExecLegByParams != null)
            {
                if (execParams.ExecLegByParams.LegByTriggerType == HostingXQComposeLimitOrderLegByTriggerType.PRICE_BEST)
                    return XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_BEST;
                else if (execParams.ExecLegByParams.LegByTriggerType == HostingXQComposeLimitOrderLegByTriggerType.PRICE_TRYING)
                    return XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING;
            }

            return null;
        }
    }
}
