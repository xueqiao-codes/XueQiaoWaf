using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderHistoryParkedListVM : ViewModel<IOrderHistoryParkedListView>
    {
        [ImportingConstructor]
        protected OrderHistoryParkedListVM(IOrderHistoryParkedListView view) : base(view)
        {
            OrderList = new ObservableCollection<OrderItemDataModel_Parked>();
            OrderListCollectionView = CollectionViewSource.GetDefaultView(OrderList) as ListCollectionView;
            ParkedOrderRealtimeListDataContext.CommonConfigParkedOrderListCollectionView(OrderListCollectionView,
                new SortDescription(nameof(OrderItemDataModel_Parked.OrderTimestampMs), ListSortDirection.Descending));
        }
        
        private ICommand toShowChildOrderCmd;
        public ICommand ToShowChildOrderCmd
        {
            get { return toShowChildOrderCmd; }
            set { SetProperty(ref toShowChildOrderCmd, value); }
        }
        
        /// <summary>
        /// 点击项目的唯一标识相关列 command
        /// cmd param: <see cref="OrderItemDataModel"/>类型
        /// </summary>
        private ICommand clickItemTargetKeyRelatedColumnCmd;
        public ICommand ClickItemTargetKeyRelatedColumnCmd
        {
            get { return clickItemTargetKeyRelatedColumnCmd; }
            set { SetProperty(ref clickItemTargetKeyRelatedColumnCmd, value); }
        }

        public ObservableCollection<OrderItemDataModel_Parked> OrderList { get; private set; }
        public ListCollectionView OrderListCollectionView { get; private set; }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public IEnumerable<ListColumnInfo> ListDisplayColumnInfos => ViewCore.ListDisplayColumnInfos;

        public void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos)
        {
            ViewCore.ResetListDisplayColumns(listColumnInfos);
        }
    }
}
