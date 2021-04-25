using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class FundItemsService
    {
        public FundItemsService()
        {
            FundItems = new ObservableCollection<FundItemDataModel>();
        }

        public ObservableCollection<FundItemDataModel> FundItems { get; private set; }
    }
}
