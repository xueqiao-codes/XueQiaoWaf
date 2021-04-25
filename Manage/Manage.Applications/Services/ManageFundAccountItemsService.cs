using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting;

namespace Manage.Applications.Services
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class ManageFundAccountItemsService
    {
        public ManageFundAccountItemsService()
        {
            AccountItems = new ObservableCollection<HostingTradeAccount>();
        }

        public ObservableCollection<HostingTradeAccount> AccountItems { get; private set; }
    }
}
