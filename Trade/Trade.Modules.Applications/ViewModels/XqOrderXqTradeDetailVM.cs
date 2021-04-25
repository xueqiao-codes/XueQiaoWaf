using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class XqOrderXqTradeDetailVM : ViewModel<IXqOrderXqTradeDetailView>
    {
        [ImportingConstructor]
        protected XqOrderXqTradeDetailVM(IXqOrderXqTradeDetailView view) : base(view)
        {
            XqComposeWaitTradeItems = new ObservableCollection<XqTradeDetailDM>();
            XqTradeDetailItems = new ObservableCollection<XqTradeDetailDM>();
        }

        public void InvalidateViewWithOrderTargetType(ClientXQOrderTargetType orderTargetType)
        {
            ViewCore.InvalidateViewWithOrderTargetType(orderTargetType);
        }

        public ObservableCollection<XqTradeDetailDM> XqComposeWaitTradeItems { get; private set; }

        public ObservableCollection<XqTradeDetailDM> XqTradeDetailItems { get; private set; }

        private ICommand showXqTradeExecTradesCmd;
        public ICommand ShowXqTradeExecTradesCmd
        {
            get { return showXqTradeExecTradesCmd; }
            set { SetProperty(ref showXqTradeExecTradesCmd, value); }
        }

    }
}
