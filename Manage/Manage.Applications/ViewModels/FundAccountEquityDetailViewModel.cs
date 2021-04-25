using Manage.Applications.DataModels;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class FundAccountEquityDetailViewModel : ViewModel<IFundAccountEquityDetailView>
    {
        [ImportingConstructor]
        protected FundAccountEquityDetailViewModel(IFundAccountEquityDetailView view) : base(view)
        {
            Details = new ObservableCollection<FundAccountEquityModel>();
        }

        public ObservableCollection<FundAccountEquityModel> Details { get; private set; }
    }
}
