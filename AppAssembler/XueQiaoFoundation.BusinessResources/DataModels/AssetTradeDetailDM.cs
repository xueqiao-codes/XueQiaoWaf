using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting.asset.thriftapi;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class AssetTradeDetailDM : Model
    {
        public AssetTradeDetailDM(AssetTradeDetail detail, long subUserId)
        {
            this.Detail = detail;
            this.SubUserId = subUserId;

            if (!string.IsNullOrEmpty(detail?.Source))
            {
                if (Enum.TryParse(detail.Source, out TradeDetailSource source))
                {
                    this.Source = source;
                }
            }
        }

        public AssetTradeDetail Detail { get; private set; }

        /// <summary>
        /// 来源用户 id
        /// </summary>
        public long SubUserId { get; private set; }

        /// <summary>
        /// 来源用户名称
        /// </summary>
        private string subUserName;
        public string SubUserName
        {
            get { return subUserName; }
            set { SetProperty(ref subUserName, value); }
        }

        /// <summary>
        /// 来源类型
        /// </summary>
        private TradeDetailSource? source;
        public TradeDetailSource? Source
        {
            get { return source; }
            set { SetProperty(ref source, value); }
        }
    }

    public enum TradeDetailSource
    {
        XQ_TRADE = 0,   // 雪橇成交
        ASSIGN = 1,     // 分配
    }
}
