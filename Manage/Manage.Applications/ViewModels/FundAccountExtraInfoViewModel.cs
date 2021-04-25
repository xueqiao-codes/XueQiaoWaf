using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class FundAccountExtraInfoViewModel : ViewModel<IFundAccountExtraInfoView>
    {
        [ImportingConstructor]
        protected FundAccountExtraInfoViewModel(IFundAccountExtraInfoView view) : base(view)
        {
        }

        private ICommand closePageCmd;
        public ICommand ClosePageCmd
        {
            get { return closePageCmd; }
            set { SetProperty(ref closePageCmd, value); }
        }
        
        private Tuple<string,string>[] extraInfoKVs;
        public Tuple<string,string>[] ExtraInfoKVs
        {
            get { return extraInfoKVs; }
            set { SetProperty(ref extraInfoKVs, value); }
        }
    }
}
