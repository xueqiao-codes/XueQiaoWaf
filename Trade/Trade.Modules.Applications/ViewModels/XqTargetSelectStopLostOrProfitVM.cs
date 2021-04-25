using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqTargetSelectStopLostOrProfitVM : ViewModel<IXqTargetSelectStopLostOrProfitView>
    {
        [ImportingConstructor]
        protected XqTargetSelectStopLostOrProfitVM(IXqTargetSelectStopLostOrProfitView view) : base(view)
        {
            ToStopLostBuyCmd = new DelegateCommand(() => this.ToStopLostBuyHandler?.Invoke());
            ToStopLostSellCmd = new DelegateCommand(() => this.ToStopLostSellHandler?.Invoke());
            ToStopProfitBuyCmd = new DelegateCommand(() => this.ToStopProfitBuyHandler?.Invoke());
            ToStopProfitSellCmd = new DelegateCommand(() => this.ToStopProfitSellHandler?.Invoke());
        }

        public Action ToStopLostBuyHandler { get; set; }

        public Action ToStopLostSellHandler { get; set; }

        public Action ToStopProfitBuyHandler { get; set; }

        public Action ToStopProfitSellHandler { get; set; }

        public DelegateCommand ToStopLostBuyCmd { get; private set; }

        public DelegateCommand ToStopLostSellCmd { get; private set; }

        public DelegateCommand ToStopProfitBuyCmd { get; private set; }

        public DelegateCommand ToStopProfitSellCmd { get; private set; }
    }
}
