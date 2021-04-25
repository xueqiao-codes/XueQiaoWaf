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
    public class PlaceOrderCreateMainViewModel : ViewModel<IPlaceOrderCreateMainView>
    {
        [ImportingConstructor]
        protected PlaceOrderCreateMainViewModel(IPlaceOrderCreateMainView view) : base(view)
        {
            SupportPlaceOrderTypes = new ObservableCollection<ClientPlaceOrderType>();
        }

        private PlaceOrderViewCreateDramaBase placeOrderViewCreateDrama;
        /// <summary>
        /// 下单视图创建剧本
        /// </summary>
        public PlaceOrderViewCreateDramaBase PlaceOrderViewCreateDrama
        {
            get { return placeOrderViewCreateDrama; }
            set { SetProperty(ref placeOrderViewCreateDrama, value); }
        }

        /// <summary>
        /// 支持的下单类型列表
        /// </summary>
        public ObservableCollection<ClientPlaceOrderType> SupportPlaceOrderTypes { get; private set; }

        private ClientPlaceOrderType? selectedPlaceOrderType = ClientPlaceOrderType.PRICE_LIMIT;
        // 选中的下单类型
        public ClientPlaceOrderType? SelectedPlaceOrderType
        {
            get { return selectedPlaceOrderType; }
            set { SetProperty(ref selectedPlaceOrderType, value); }
        }

        private double orderTargetPriceTickSize;
        // 下单目标的价格 tick
        public double OrderTargetPriceTickSize
        {
            get { return orderTargetPriceTickSize; }
            set { SetProperty(ref orderTargetPriceTickSize, value); }
        }

        private int orderQuantity = 1;
        // 下单数量
        public int OrderQuantity
        {
            get { return orderQuantity; }
            set { SetProperty(ref orderQuantity, value); }
        }
        
        private ICommand bidPriceClickCommand;
        public ICommand BidPriceClickCommand
        {
            get { return bidPriceClickCommand; }
            set { SetProperty(ref bidPriceClickCommand, value); }
        }

        private ICommand askPriceClickCommand;
        public ICommand AskPriceClickCommand
        {
            get { return askPriceClickCommand; }
            set { SetProperty(ref askPriceClickCommand, value); }
        }

        private ICommand buyCommand;
        public ICommand BuyCommand
        {
            get { return buyCommand; }
            set { SetProperty(ref buyCommand, value); }
        }

        private ICommand sellCommand;
        public ICommand SellCommand
        {
            get { return sellCommand; }
            set { SetProperty(ref sellCommand, value); }
        }

        private double? bidPrice1;
        // 最新买价
        public double? BidPrice1
        {
            get { return bidPrice1; }
            set { SetProperty(ref bidPrice1, value); }
        }
        
        private long? bidQty1;
        // 最新买量
        public long? BidQty1
        {
            get { return bidQty1; }
            set { SetProperty(ref bidQty1, value); }
        }

        private double? askPrice1;
        // 最新卖价
        public double? AskPrice1
        {
            get { return askPrice1; }
            set { SetProperty(ref askPrice1, value); }
        }

        private long? askQty1;
        // 最新卖量
        public long? AskQty1
        {
            get { return askQty1; }
            set { SetProperty(ref askQty1, value); }
        }
    }
}
