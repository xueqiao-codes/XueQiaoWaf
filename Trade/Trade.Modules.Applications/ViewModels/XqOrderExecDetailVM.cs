using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqOrderExecDetailVM : ViewModel<IXqOrderExecDetailView>
    {
        [ImportingConstructor]
        protected XqOrderExecDetailVM(IXqOrderExecDetailView view) : base(view)
        {
            ExecOrderItems = new ObservableCollection<ExecOrderDM>();
        }

        public void InvalidateViewWithOrderTargetType(ClientXQOrderTargetType orderTargetType)
        {
            ViewCore.InvalidateViewWithOrderTargetType(orderTargetType);
        }

        public ObservableCollection<ExecOrderDM> ExecOrderItems { get; private set; }

        private object execTradeItemsView;
        /// <summary>
        /// 执行成交列表视图
        /// </summary>
        public object ExecTradeItemsView
        {
            get { return execTradeItemsView; }
            set { SetProperty(ref execTradeItemsView, value); }
        }

    }
}
