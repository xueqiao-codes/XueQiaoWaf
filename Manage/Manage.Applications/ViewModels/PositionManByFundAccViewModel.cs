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
    public class PositionManByFundAccViewModel : ViewModel<IPositionManByFundAccView>
    {
        [ImportingConstructor]
        protected PositionManByFundAccViewModel(IPositionManByFundAccView view) : base(view)
        {
        }
    }
}
