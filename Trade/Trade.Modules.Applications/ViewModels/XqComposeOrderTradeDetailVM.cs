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
    public class XqComposeOrderTradeDetailVM : ViewModel<IXqContractOrderTradeDetailView>
    {
        [ImportingConstructor]
        protected XqComposeOrderTradeDetailVM(IXqContractOrderTradeDetailView view) : base(view)
        {
        }
    }
}
