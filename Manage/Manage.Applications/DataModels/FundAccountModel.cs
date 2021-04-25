using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting;

namespace Manage.Applications.DataModels
{
    public class FundAccountModel : Model
    {
        public FundAccountModel(long accountId)
        {
            this.AccountId = accountId;
        }

        public long AccountId { get; private set; }

        private HostingTradeAccount accountMeta;
        public HostingTradeAccount AccountMeta
        {
            get { return accountMeta; }
            set { SetProperty(ref accountMeta, value); }
        }
        
        private string brokerAccessName;
        public string BrokerAccessName
        {
            get { return brokerAccessName; }
            set { SetProperty(ref brokerAccessName, value); }
        }
        
        private string brokerName;
        public string BrokerName
        {
            get { return brokerName; }
            set { SetProperty(ref brokerName, value); }
        }

        /// <summary>
        /// 账号的其他信息的格式化
        /// </summary>
        private string formatExtraAccountPropertiesMsg;
        public string FormatExtraAccountPropertiesMsg
        {
            get { return formatExtraAccountPropertiesMsg; }
            set { SetProperty(ref formatExtraAccountPropertiesMsg, value); }
        }

    }
}
