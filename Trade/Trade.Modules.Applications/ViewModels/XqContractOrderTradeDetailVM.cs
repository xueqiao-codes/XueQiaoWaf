using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    /// <summary>
    /// 雪橇合约标的订单的成交详情
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqContractOrderTradeDetailVM : ViewModel<IXqContractOrderTradeDetailView>
    {
        [ImportingConstructor]
        protected XqContractOrderTradeDetailVM(IXqContractOrderTradeDetailView view) : base(view)
        {
            XqTradeItems = new ObservableCollection<TradeItemDataModel>();
        }

        public ObservableCollection<TradeItemDataModel> XqTradeItems { get; private set; }
    }
}
