using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 雪橇标的明细持仓项 data model
    /// </summary>
    public class XqTargetDetailPositionDM : Model
    {
        public XqTargetDetailPositionDM(string targetKey, ClientXQOrderTargetType targetType, long subAccountId)
        {
            this.TargetKey = targetKey;
            this.TargetType = targetType;
            this.SubAccountId = subAccountId;
        }

        public string TargetKey { get; private set; }

        public ClientXQOrderTargetType TargetType { get; private set; }

        public long SubAccountId { get; private set; }
        
        private ClientTradeDirection direction;
        public ClientTradeDirection Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

        private long sourceDataTimestampMs;
        /// <summary>
        /// 数据生成时间
        /// </summary>
        public long SourceDataTimestampMs
        {
            get { return sourceDataTimestampMs; }
            set { SetProperty(ref sourceDataTimestampMs, value); }
        }

        private XqTargetPositionDataSourceChannel? sourceDataChannel;
        /// <summary>
        /// 数据来源渠道
        /// </summary>
        public XqTargetPositionDataSourceChannel? SourceDataChannel
        {
            get { return sourceDataChannel; }
            set { SetProperty(ref sourceDataChannel, value); }
        }

#region 明细持仓项显示列
        public const string XqTargetDetailPositionColumn_Direction = "XqTargetDetailPositionColumn_Direction";
        public const string XqTargetDetailPositionColumn_Quantity = "XqTargetDetailPositionColumn_Quantity";
        public const string XqTargetDetailPositionColumn_Price = "XqTargetDetailPositionColumn_Price";
        public const string XqTargetDetailPositionColumn_SourceDataTime = "XqTargetDetailPositionColumn_SourceDataTime";
        public const string XqTargetDetailPositionColumn_SourceDataChannel = "XqTargetDetailPositionColumn_SourceDataChannel";
        public const string XqTargetDetailPositionColumn_Detail = "XqTargetDetailPositionColumn_Detail";

        /// <summary>
        /// 合约持仓明细列表显示的列
        /// </summary>
        public static readonly string[] XqContractTargetDetailPositionColumns =
            new string[] 
            {
                XqTargetDetailPositionColumn_Direction, XqTargetDetailPositionColumn_Quantity,
                XqTargetDetailPositionColumn_Price, XqTargetDetailPositionColumn_SourceDataTime,
                XqTargetDetailPositionColumn_SourceDataChannel
            };

        /// <summary>
        /// 组合持仓明细列表显示的列
        /// </summary>
        public static readonly string[] XqComposeTargetDetailPositionColumns =
            new string[]
            {
                XqTargetDetailPositionColumn_Direction, XqTargetDetailPositionColumn_Quantity,
                XqTargetDetailPositionColumn_Price, XqTargetDetailPositionColumn_SourceDataTime,
                XqTargetDetailPositionColumn_SourceDataChannel, XqTargetDetailPositionColumn_Detail
            };
        #endregion
    }
}
