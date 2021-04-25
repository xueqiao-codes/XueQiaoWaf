using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubAccountSettlementContainerVM : ViewModel<ISubAccountSettlementContainerView>
    {
        [ImportingConstructor]
        public SubAccountSettlementContainerVM(ISubAccountSettlementContainerView view) : base(view)
        {
        }
    }
}
