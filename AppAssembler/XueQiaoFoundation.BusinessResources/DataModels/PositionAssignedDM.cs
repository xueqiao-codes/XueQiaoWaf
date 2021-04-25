using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting.position.adjust.assign.thriftapi;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 持仓分配项 data model
    /// </summary>
    public class PositionAssignedDM : Model
    {
        public PositionAssignedDM(long assignId, long subAccountId, 
            long inputSubUserId, long assignSubUserId, long tradeAccountId, long sledContractId)
        {
            this.AssignId = assignId;
            this.SubAccountId = subAccountId;
            this.InputSubUserId = inputSubUserId;
            this.AssignSubUserId = assignSubUserId;
            this.TradeAccountId = tradeAccountId;
            this.ContractId = sledContractId;
            this.ContractDetailContainer = new TargetContract_TargetContractDetail((int)sledContractId);
        }

        /// <summary>
        /// 分配 id
        /// </summary>
        public long AssignId { get; private set; }

        /// <summary>
        /// 分配至的操作账户 id
        /// </summary>
        public long SubAccountId { get; private set; }

        /// <summary>
        /// 持仓录入者 user id
        /// </summary>
        public long InputSubUserId { get; private set; }

        /// <summary>
        /// 持仓分配者 user id
        /// </summary>
        public long AssignSubUserId { get; private set; }

        /// <summary>
        /// 资金账户 id
        /// </summary>
        public long TradeAccountId { get; private set; }

        /// <summary>
        /// 雪橇合约 id
        /// </summary>
        public long ContractId { get; private set; }

        private double? price;
        public double? Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private int? volume;
        public int? Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }

        private ClientTradeDirection? direction;
        public ClientTradeDirection? Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }

        /// <summary>
        /// 持仓生效时间
        /// </summary>
        private long? positionTimestampMs;
        public long? PositionTimestampMs
        {
            get { return positionTimestampMs; }
            set { SetProperty(ref positionTimestampMs, value); }
        }

        private long? createTimestampMs;
        public long? CreateTimestampMs
        {
            get { return createTimestampMs; }
            set { SetProperty(ref createTimestampMs, value); }
        }

        private long? lastmodifyTimestampMs;
        public long? LastmodifyTimestampMs
        {
            get { return lastmodifyTimestampMs; }
            set { SetProperty(ref lastmodifyTimestampMs, value); }
        }


        /// <summary>
        /// 分配至的操作账户名称
        /// </summary>
        private string subAccountName;
        public string SubAccountName
        {
            get { return subAccountName; }
            set { SetProperty(ref subAccountName, value); }
        }

        /// <summary>
        /// 持仓录入者名称
        /// </summary>
        private string inputUserName;
        public string InputUserName
        {
            get { return inputUserName; }
            set { SetProperty(ref inputUserName, value); }
        }

        /// <summary>
        /// 持仓分配者名称
        /// </summary>
        private string assignUserName;
        public string AssignUserName
        {
            get { return assignUserName; }
            set { SetProperty(ref assignUserName, value); }
        }

        /// <summary>
        /// 资金账户名称
        /// </summary>
        private string tradeAccountName;
        public string TradeAccountName
        {
            get { return tradeAccountName; }
            set { SetProperty(ref tradeAccountName, value); }
        }

        /// <summary>
        /// 合约详情容器
        /// </summary>
        public TargetContract_TargetContractDetail ContractDetailContainer { get; private set; }


        public static PositionAssignedDM FromIDLItem(PositionAssigned idlItem)
        {
            if (idlItem == null) return null;
            var tar = new PositionAssignedDM(idlItem.AssignId, idlItem.SubAccountId,
                idlItem.InputSubUserId, idlItem.AssignSubUserId, idlItem.TradeAccountId, idlItem.SledContractId);
            if (idlItem.__isset.price) tar.Price = idlItem.Price;
            if (idlItem.__isset.volume) tar.Volume = idlItem.Volume;
            if (idlItem.__isset.positionDirection)
                tar.Direction = (idlItem.PositionDirection == PositionDirection.POSITION_BUY) ? ClientTradeDirection.BUY: ClientTradeDirection.SELL;
            if (idlItem.__isset.positionTimestampMs) tar.PositionTimestampMs = idlItem.PositionTimestampMs;
            if (idlItem.__isset.createTimestampMs) tar.CreateTimestampMs = idlItem.CreateTimestampMs;
            if (idlItem.__isset.lastmodifyTimestampMs) tar.LastmodifyTimestampMs = idlItem.LastmodifyTimestampMs;
            return tar;
        }
    }
}
