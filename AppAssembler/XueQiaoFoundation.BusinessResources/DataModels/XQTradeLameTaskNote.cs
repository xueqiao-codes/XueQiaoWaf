using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class XQTradeLameTaskNote : Model
    {
        public XQTradeLameTaskNote(long subAccountId, long XQTradeId)
        {
            this.SubAccountId = subAccountId;
            this.XQTradeId = XQTradeId;
        }

        /// <summary>
        /// 操作账户 id
        /// </summary>
        public long SubAccountId { get; private set; }

        /// <summary>
        /// 雪橇成交 id
        /// </summary>
        public long XQTradeId { get; private set; }

        private TradeItemDataModel tradeInfo;
        /// <summary>
        /// 成交项信息
        /// </summary>
        public TradeItemDataModel TradeInfo
        {
            get { return tradeInfo; }
            set { SetProperty(ref tradeInfo, value); }
        }
        
        private long createTimestampMs;
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTimestampMs
        {
            get { return createTimestampMs; }
            set { SetProperty(ref createTimestampMs, value); }
        }
    }
}
