using Manage.Applications.DataModels;
using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqTradeItemsCompareViewModel : ViewModel<IXqTradeItemsCompareView>
    {
        [ImportingConstructor]
        protected XqTradeItemsCompareViewModel(IXqTradeItemsCompareView view) : base(view)
        {
        }

        private SettlementCompareItem settlementCompareItem;
        public SettlementCompareItem SettlementCompareItem
        {
            get { return settlementCompareItem; }
            set { SetProperty(ref settlementCompareItem, value); }
        }

        private ICommand triggerQueryCmd;
        public ICommand TriggerQueryCmd
        {
            get { return triggerQueryCmd; }
            set { SetProperty(ref triggerQueryCmd, value); }
        }
        
        private bool showOpenInAnotherWindowButton;
        public bool ShowOpenInAnotherWindowButton
        {
            get { return showOpenInAnotherWindowButton; }
            set { SetProperty(ref showOpenInAnotherWindowButton, value); }
        }

        private ICommand toOpenInAnotherWindowCmd;
        public ICommand ToOpenInAnotherWindowCmd
        {
            get { return toOpenInAnotherWindowCmd; }
            set { SetProperty(ref toOpenInAnotherWindowCmd, value); }
        }

        private object xqTradeItemsPagerView;
        public object XqTradeItemsPagerView
        {
            get { return xqTradeItemsPagerView; }
            set { SetProperty(ref xqTradeItemsPagerView, value); }
        }
    }
}
