using NativeModel.Trade;
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
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaceOrderComponentContentViewModel : ViewModel<IPlaceOrderComponentContentView>
    {
        private readonly XqTargetPresentOrderListCtrl xqTargetPresentOrderListCtrl;
        private PlaceOrderComponentPresentKey currentPresentKey;

        [ImportingConstructor]
        public PlaceOrderComponentContentViewModel(IPlaceOrderComponentContentView view,
            XqTargetPresentOrderListCtrl xqTargetPresentOrderListCtrl) : base(view)
        {
            this.xqTargetPresentOrderListCtrl = xqTargetPresentOrderListCtrl;
        }
        
        /// <summary>
        /// 下单部分的视图宽度
        /// </summary>
        public double PlaceOrderColumnViewWidth => ViewCore.PlaceOrderColumnViewWidth;

        /// <summary>
        /// 图表部分的视图宽度
        /// </summary>
        public double ChartColumnViewWidth => ViewCore.ChartColumnViewWidth;

        private object placeOrderCreateMainView;
        // 下单主视图
        public object PlaceOrderCreateMainView
        {
            get { return placeOrderCreateMainView; }
            set { SetProperty(ref placeOrderCreateMainView, value); }
        }

        private object placeOrderCreateConditionView;
        // 下单条件视图
        public object PlaceOrderCreateConditionView
        {
            get { return placeOrderCreateConditionView; }
            set { SetProperty(ref placeOrderCreateConditionView, value); }
        }

        private object hangingOrdersAreaView;
        // 挂单区域视图
        public object HangingOrdersAreaView
        {
            get { return hangingOrdersAreaView; }
            set { SetProperty(ref hangingOrdersAreaView, value); }
        }

        private object chartView;
        // 图表视图
        public object ChartView
        {
            get { return chartView; }
            set { SetProperty(ref chartView, value); }
        }
        

        // 最新价
        private double? lastPrice;
        public double? LastPrice
        {
            get { return lastPrice; }
            set { SetProperty(ref lastPrice, value); }
        }

        // 最高价
        private double? highPrice;
        public double? HighPrice
        {
            get { return highPrice; }
            set { SetProperty(ref highPrice, value); }
        }

        // 开盘价
        private double? openPrice;
        public double? OpenPrice
        {
            get { return openPrice; }
            set { SetProperty(ref openPrice, value); }
        }

        // 成交量
        private long? volume;
        public long? Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }

        // 涨停价
        private double? upperLimitPrice;
        public double? UpperLimitPrice
        {
            get { return upperLimitPrice; }
            set { SetProperty(ref upperLimitPrice, value); }
        }

        // 最低价
        private double? lowPrice;
        public double? LowPrice
        {
            get { return lowPrice; }
            set { SetProperty(ref lowPrice, value); }
        }

        // 昨收价
        private double? preClosePrice;
        public double? PreClosePrice
        {
            get { return preClosePrice; }
            set { SetProperty(ref preClosePrice, value); }
        }

        // 成交额
        private double? turnover;
        public double? Turnover
        {
            get { return turnover; }
            set { SetProperty(ref turnover, value); }
        }

        // 跌停价
        private double? lowerLimitPrice;
        public double? LowerLimitPrice
        {
            get { return lowerLimitPrice; }
            set { SetProperty(ref lowerLimitPrice, value); }
        }

        private double? bidPrice1;
        public double? BidPrice1
        {
            get { return bidPrice1; }
            set { SetProperty(ref bidPrice1, value); }
        }

        private long? bidQty1;
        public long? BidQty1
        {
            get { return bidQty1; }
            set { SetProperty(ref bidQty1, value); }
        }

        private double? askPrice1;
        public double? AskPrice1
        {
            get { return askPrice1; }
            set { SetProperty(ref askPrice1, value); }
        }

        private long? askQty1;
        public long? AskQty1
        {
            get { return askQty1; }
            set { SetProperty(ref askQty1, value); }
        }

        private long? targetPositionVolume;
        /// <summary>
        /// 标的持仓量
        /// </summary>
        public long? TargetPositionVolume
        {
            get { return targetPositionVolume; }
            set { SetProperty(ref targetPositionVolume, value); }
        }

        private double? targetPositionAvgPrice;
        /// <summary>
        /// 标的持仓均价
        /// </summary>
        public double? TargetPositionAvgPrice
        {
            get { return targetPositionAvgPrice; }
            set { SetProperty(ref targetPositionAvgPrice, value); }
        }

        private double? targetProfitLoss;
        /// <summary>
        /// 标的盈亏
        /// </summary>
        public double? TargetProfitLoss
        {
            get { return targetProfitLoss; }
            set { SetProperty(ref targetProfitLoss, value); }
        }
        
        private bool placeOrderPartShowing;
        public bool PlaceOrderPartShowing
        {
            get { return placeOrderPartShowing; }
            set { SetProperty(ref placeOrderPartShowing, value); }
        }

        private bool chartPartShowing;
        public bool ChartPartShowing
        {
            get { return chartPartShowing; }
            set { SetProperty(ref chartPartShowing, value); }
        }
        
        private ICommand showTargetConditionOrdersCmd;
        public ICommand ShowTargetConditionOrdersCmd
        {
            get { return showTargetConditionOrdersCmd; }
            set { SetProperty(ref showTargetConditionOrdersCmd, value); }
        }

        private ICommand showTargetParkedOrdersCmd;
        public ICommand ShowTargetParkedOrdersCmd
        {
            get { return showTargetParkedOrdersCmd; }
            set { SetProperty(ref showTargetParkedOrdersCmd, value); }
        }

        public void UpdateViewPresentKey(PlaceOrderComponentPresentKey presentKey)
        {
            if (this.currentPresentKey == presentKey) return;
            this.currentPresentKey = presentKey;
            this.TargetType = presentKey?.TargetType;
            
            InvalidateTargetOrderLists();
        }
        
        private ClientXQOrderTargetType? targetType;
        public ClientXQOrderTargetType? TargetType
        {
            get { return targetType; }
            private set { SetProperty(ref targetType, value); }
        }

        private ObservableCollection<OrderItemDataModel> targetConditionOrders;
        public ObservableCollection<OrderItemDataModel> TargetConditionOrders
        {
            get { return targetConditionOrders; }
            private set { SetProperty(ref targetConditionOrders, value); }
        }

        private ObservableCollection<OrderItemDataModel> targetParkedOrders;
        public ObservableCollection<OrderItemDataModel> TargetParkedOrders
        {
            get { return targetParkedOrders; }
            private set { SetProperty(ref targetParkedOrders, value); }
        }

        private void InvalidateTargetOrderLists()
        {
            var subAccountId = this.currentPresentKey?.SubAccountId;
            var targetType = this.currentPresentKey?.TargetType;
            var targetKey = this.currentPresentKey?.TargetKey;
            if (subAccountId != null && targetType != null && !string.IsNullOrEmpty(targetKey))
            {
                var conditionOrdersKey = new XqTargetPresentOrderListKey(subAccountId.Value, targetType.Value, targetKey, XQClientOrderType.Condition);
                this.TargetConditionOrders = xqTargetPresentOrderListCtrl.TryGetXqTargetPresentOrderList(conditionOrdersKey);
                var parkedOrdersKey = new XqTargetPresentOrderListKey(subAccountId.Value, targetType.Value, targetKey, XQClientOrderType.Parked);
                this.TargetParkedOrders = xqTargetPresentOrderListCtrl.TryGetXqTargetPresentOrderList(parkedOrdersKey);
            }
            else
            {
                this.TargetConditionOrders = null;
                this.TargetParkedOrders = null;
            }
        }
    }
}
