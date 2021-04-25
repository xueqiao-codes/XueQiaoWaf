using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Interfaces.Services;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    /// <summary>
    /// 成交数据
    /// </summary>
    [Export, Export(typeof(ITradeItemsService)), PartCreationPolicy(CreationPolicy.Shared)]
    public class TradeItemsService : ITradeItemsService
    {
        public TradeItemsService()
        {
            TradeItems = new ObservableCollection<TradeItemDataModel>();
        }

        /// <summary>
        /// 成交列表
        /// </summary>
        public ObservableCollection<TradeItemDataModel> TradeItems { get; private set; }
        
    }
}
