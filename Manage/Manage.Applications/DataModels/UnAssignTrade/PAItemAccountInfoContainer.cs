using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 预分配项账户信息容器
    /// </summary>
    public class PAItemAccountInfoContainer : Model
    {
        public PAItemAccountInfoContainer(long fundAccountId, long subAccountId)
        {
            this.FundAccountId = fundAccountId;
            this.SubAccountId = subAccountId;
        }

        public long FundAccountId { get; private set; }

        public long SubAccountId { get; private set; }

        private HostingSubAccount subAccount;
        public HostingSubAccount SubAccount
        {
            get { return subAccount; }
            set { SetProperty(ref subAccount, value); }
        }

        private HostingTradeAccount fundAccount;
        public HostingTradeAccount FundAccount
        {
            get { return fundAccount; }
            set { SetProperty(ref fundAccount, value); }
        }
    }
}
