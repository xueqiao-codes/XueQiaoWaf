using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class DisplayColumnsOrderListEntrustedConfigItemsContainerModel : DisplayColumnsConfigItemsContainerModelBase<IDisplayColumnsOrderListEntrustedConfigItemsContainerView>
    {
        [ImportingConstructor]
        protected DisplayColumnsOrderListEntrustedConfigItemsContainerModel(IDisplayColumnsOrderListEntrustedConfigItemsContainerView view) : base(view)
        {
        }
    }
}
