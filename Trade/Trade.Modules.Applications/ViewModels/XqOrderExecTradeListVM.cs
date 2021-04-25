using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqOrderExecTradeListVM : ViewModel<IXqOrderExecTradeListView>
    {
        [ImportingConstructor]
        protected XqOrderExecTradeListVM(IXqOrderExecTradeListView view) : base(view)
        {
            ExecTradeItems = new ObservableCollection<ExecTradeDM>();
        }

        public void InvalidateViewWithOrderTargetType(ClientXQOrderTargetType orderTargetType)
        {
            ViewCore.UpdateListColumnsByPresentTarget(orderTargetType);
        }

        public ObservableCollection<ExecTradeDM> ExecTradeItems { get; private set; }
        
        private Thickness viewMargin;
        public Thickness ViewMargin
        {
            get { return viewMargin; }
            set { SetProperty(ref viewMargin, value); }
        }
    }
}
