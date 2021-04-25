using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Services
{
    public interface ITradeItemsService
    {
        /// <summary>
        /// 成交列表
        /// </summary>
        ObservableCollection<TradeItemDataModel> TradeItems { get; }
    }
}
