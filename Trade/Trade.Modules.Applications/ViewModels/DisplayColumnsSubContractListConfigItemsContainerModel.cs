using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class DisplayColumnsSubContractListConfigItemsContainerModel : DisplayColumnsConfigItemsContainerModelBase<IDisplayColumnsSubContractListConfigItemsContainerView>
    {
        [ImportingConstructor]
        protected DisplayColumnsSubContractListConfigItemsContainerModel(IDisplayColumnsSubContractListConfigItemsContainerView view) : base(view)
        {
        }
    }
}
