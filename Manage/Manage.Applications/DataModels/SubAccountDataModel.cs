using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting;

namespace Manage.Applications.DataModels
{
    public class SubAccountDataModel : Model
    {
        public SubAccountDataModel(long subAccountId)
        {
            this.SubAccountId = subAccountId;
            AuthedToSubUsers = new ObservableCollection<HostingSubAccountRelatedItem>();
        }

        public long SubAccountId { get; private set; }

        private HostingSubAccount accountMeta;
        public HostingSubAccount AccountMeta
        {
            get { return accountMeta; }
            set { SetProperty(ref accountMeta, value); }
        }
        
        public ObservableCollection<HostingSubAccountRelatedItem> AuthedToSubUsers { get; private set; }
    }
}
