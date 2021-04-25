using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace NativeModel.Trade
{
    public class NativeQuotationItem : Model
    {
        public NativeQuotationItem()
        {
            InvalidateIncreasePriceAndRate();
            InvalidateDailyIncrementOpenInterest();
        }
        
        /// <summary>
        /// 计算涨跌和涨跌幅
        /// </summary>
        /// <param name="lastPrice"></param>
        /// <param name="preSettlementPrice"></param>
        /// <param name="increasePrice"></param>
        /// <param name="increasePriceRate"></param>
        public static void CalculateIncreasePrice(double? lastPrice, double? preSettlementPrice,
            out double? increasePrice, out double? increasePriceRate)
        {
            increasePrice = null;
            increasePriceRate = null;
            if (lastPrice == null || preSettlementPrice == null) return;

            increasePrice = lastPrice - preSettlementPrice;
            if (preSettlementPrice != 0)
            {
                increasePriceRate = (lastPrice - preSettlementPrice) / preSettlementPrice;
            }
        }
        
        private void InvalidateDailyIncrementOpenInterest()
        {
            var _openInterest = OpenInterest;
            var _preOpenInterest = PreOpenInterest;

            if (_openInterest == null || _preOpenInterest == null)
                this.DailyIncrementOpenInterest = null;
            else 
                this.DailyIncrementOpenInterest = _openInterest - _preOpenInterest;
        }

        private void InvalidateIncreasePriceAndRate()
        {
            CalculateIncreasePrice(LastPrice, PreSettlementPrice, out double? increasePrice, out double? increasePriceRate);
            this.IncreasePrice = increasePrice;
            this.IncreasePriceRate = increasePriceRate;
        }

        public ContractSymbol ContractSymbol { set; get; }

        // 更新时间
        private long updateTimestampMs;
        public long UpdateTimestampMs
        {
            get { return updateTimestampMs; }
            set { SetProperty(ref updateTimestampMs, value); }
        }

        // 最新价
        private double? lastPrice;
        public double? LastPrice
        {
            get { return lastPrice; }
            set
            {
                if (SetProperty(ref lastPrice, value))
                {
                    InvalidateIncreasePriceAndRate();
                }
            }
        }

        // 最新量
        private long? lastQty;
        public long? LastQty
        {
            get { return lastQty; }
            set { SetProperty(ref lastQty, value); }
        }

        // 买量
        private ObservableCollection<long> bidQty;
        public ObservableCollection<long> BidQty
        {
            get { return bidQty; }
            set { SetProperty(ref bidQty, value); }
        }

        // 买价
        private ObservableCollection<double> bidPrice;
        public ObservableCollection<double> BidPrice
        {
            get { return bidPrice; }
            set { SetProperty(ref bidPrice, value); }
        }

        // 卖价
        private ObservableCollection<double> askPrice;
        public ObservableCollection<double> AskPrice
        {
            get { return askPrice; }
            set { SetProperty(ref askPrice, value); }
        }

        // 卖量
        private ObservableCollection<long> askQty;
        public ObservableCollection<long> AskQty
        {
            get { return askQty; }
            set { SetProperty(ref askQty, value); }
        }

        // 涨跌（最新价 – 昨结算价）
        private double? increasePrice;
        public double? IncreasePrice
        {
            get { return increasePrice; }
            set { SetProperty(ref increasePrice, value); }
        }
        
        // 涨幅（（最新价-昨结算价）/昨结算价）
        private double? increasePriceRate;
        public double? IncreasePriceRate
        {
            get { return increasePriceRate; }
            set { SetProperty(ref increasePriceRate, value); }
        }
        
        // 成交量
        private long? volume;
        public long? Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }


        // 持仓量
        private long? openInterest;
        public long? OpenInterest
        {
            get { return openInterest; }
            set
            {
                if (SetProperty(ref openInterest, value))
                {
                    InvalidateDailyIncrementOpenInterest();
                }
            }
        }


        // 日增量（持仓量-昨持仓量）
        private long? dailyIncrementOpenInterest;
        public long? DailyIncrementOpenInterest
        {
            get { return dailyIncrementOpenInterest; }
            private set { SetProperty(ref dailyIncrementOpenInterest, value); }
        }
        
        // 开盘价
        private double? openPrice;
        public double? OpenPrice
        {
            get { return openPrice; }
            set { SetProperty(ref openPrice, value); }
        }


        // 最高价
        private double? highPrice;
        public double? HighPrice
        {
            get { return highPrice; }
            set { SetProperty(ref highPrice, value); }
        }


        // 最低价
        private double? lowPrice;
        public double? LowPrice
        {
            get { return lowPrice; }
            set { SetProperty(ref lowPrice, value); }
        }


        // 收盘价（暂时使用最新价）
        private double? closePrice;
        public double? ClosePrice
        {
            get { return closePrice; }
            set { SetProperty(ref closePrice, value); }
        }


        // 昨结算价
        private double? preSettlementPrice;
        public double? PreSettlementPrice
        {
            get { return preSettlementPrice; }
            set
            {
                if (SetProperty(ref preSettlementPrice, value))
                {
                    InvalidateIncreasePriceAndRate();
                }
            }
        }


        // 昨持仓量
        private long? preOpenInterest;
        public long? PreOpenInterest
        {
            get { return preOpenInterest; }
            set
            {
                if (SetProperty(ref preOpenInterest, value))
                {
                    InvalidateDailyIncrementOpenInterest();
                }
            }
        }


        // 昨收盘价
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
        
        // 涨停价
        private double? upperLimitPrice;
        public double? UpperLimitPrice
        {
            get { return upperLimitPrice; }
            set { SetProperty(ref upperLimitPrice, value); }
        }


        // 跌停价
        private double? lowerLimitPrice;
        public double? LowerLimitPrice
        {
            get { return lowerLimitPrice; }
            set { SetProperty(ref lowerLimitPrice, value); }
        }


        // (成交)均价
        private double? averagePrice;
        public double? AveragePrice
        {
            get { return averagePrice; }
            set { SetProperty(ref averagePrice, value); }
        }

    }
}
