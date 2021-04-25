using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ChooseLegPositions2MergeVM : ViewModel<IChooseLegPositions2MergeView>
    {
        [ImportingConstructor]
        protected ChooseLegPositions2MergeVM(IChooseLegPositions2MergeView view) : base(view)
        {
        }
        
        private CPMergeLegInfoSynchronizer mergeLegInfoSynchronizer;
        public CPMergeLegInfoSynchronizer MergeLegInfoSynchronizer
        {
            get { return mergeLegInfoSynchronizer; }
            set { SetProperty(ref mergeLegInfoSynchronizer, value); }
        }
        
        private ICommand closeDialogCmd;
        public ICommand CloseDialogCmd
        {
            get { return closeDialogCmd; }
            set { SetProperty(ref closeDialogCmd, value); }
        }

    }
}
