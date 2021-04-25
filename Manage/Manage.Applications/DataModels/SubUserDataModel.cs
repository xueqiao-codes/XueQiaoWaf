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
    public class SubUserDataModel : Model
    {
        public SubUserDataModel(int subUserId)
        {
            this.SubUserId = subUserId;
            RelatedToSubAccounts = new ObservableCollection<HostingSubAccountRelatedItem>();
        }

        public int SubUserId { get; private set; }

        private HostingUser userMeta;
        public HostingUser UserMeta
        {
            get { return userMeta; }
            set { SetProperty(ref userMeta, value); }
        }

        public ObservableCollection<HostingSubAccountRelatedItem> RelatedToSubAccounts { get; private set; }
    }
}
