using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    /// <summary>
    /// 市场item的价格相关属性值转换器
    /// </summary>
    /// <param name="propertyName">属性名称</param>
    /// <param name="sourceValue">源值</param>
    /// <returns></returns>
    public delegate T QuotationPriceRelatedPropertyValueConverter<T>(string propertyName, T sourceValue);

    public static class SubscribeDataQuotationHelper
    {
        /// <summary>
        /// 更新订阅数据的行情数据
        /// </summary>
        /// <param name="wait2UpdateQuot">要更新的行情项</param>
        /// <param name="templateQuot">用于更新的行情模板</param>
        /// <param name="pricePropertyValueConverter">价格相关属性值的转换器</param>
        /// <param name="multiValuePricePropertyValueConverter">多值价格相关属性值转换器。如AskPrice, BidPrice</param>
        public static void UpdateSubscribeDataQuotation(this NativeQuotationItem wait2UpdateQuot,
            NativeQuotationItem templateQuot,
            QuotationPriceRelatedPropertyValueConverter<double?> pricePropertyValueConverter,
            QuotationPriceRelatedPropertyValueConverter<IEnumerable<double>> multiValuePricePropertyValueConverter)
        {
            if (wait2UpdateQuot == null || templateQuot == null) return;

            // 更新时间
            wait2UpdateQuot.UpdateTimestampMs = templateQuot.UpdateTimestampMs;

            if (templateQuot.LastPrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.LastPrice), templateQuot.LastPrice)
                        ?? templateQuot.LastPrice;
                wait2UpdateQuot.LastPrice = destValue;
            }

            if (templateQuot.LastQty != null)
            {
                wait2UpdateQuot.LastQty = templateQuot.LastQty;
            }

            if (templateQuot.BidQty != null)
            {
                wait2UpdateQuot.BidQty = new ObservableCollection<long>(templateQuot.BidQty.ToArray());
            }

            if (templateQuot.BidPrice != null)
            {
                var destValue = multiValuePricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.BidPrice), templateQuot.BidPrice.ToArray())
                        ?? templateQuot.BidPrice;
                wait2UpdateQuot.BidPrice = new ObservableCollection<double>(destValue.ToArray());
            }

            if (templateQuot.AskPrice != null)
            {
                var destValue = multiValuePricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.AskPrice), templateQuot.AskPrice.ToArray())
                        ?? templateQuot.AskPrice;
                wait2UpdateQuot.AskPrice = new ObservableCollection<double>(destValue.ToArray()); ;
            }

            if (templateQuot.AskQty != null)
            {
                wait2UpdateQuot.AskQty = new ObservableCollection<long>(templateQuot.AskQty.ToArray());
            }

            if (templateQuot.Volume != null)
            {
                wait2UpdateQuot.Volume = templateQuot.Volume;
            }

            if (templateQuot.OpenInterest != null)
            {
                wait2UpdateQuot.OpenInterest = templateQuot.OpenInterest;
            }

            if (templateQuot.OpenPrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.OpenPrice), templateQuot.OpenPrice)
                        ?? templateQuot.OpenPrice;
                wait2UpdateQuot.OpenPrice = destValue;
            }

            if (templateQuot.HighPrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.HighPrice), templateQuot.HighPrice)
                        ?? templateQuot.HighPrice;
                wait2UpdateQuot.HighPrice = destValue;
            }

            if (templateQuot.LowPrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.LowPrice), templateQuot.LowPrice)
                        ?? templateQuot.LowPrice;
                wait2UpdateQuot.LowPrice = destValue;
            }

            if (templateQuot.ClosePrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.ClosePrice), templateQuot.ClosePrice)
                        ?? templateQuot.ClosePrice;
                wait2UpdateQuot.ClosePrice = destValue;
            }

            if (templateQuot.PreSettlementPrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.PreSettlementPrice), templateQuot.PreSettlementPrice)
                        ?? templateQuot.PreSettlementPrice;
                wait2UpdateQuot.PreSettlementPrice = destValue;
            }

            if (templateQuot.PreOpenInterest != null)
            {
                wait2UpdateQuot.PreOpenInterest = templateQuot.PreOpenInterest;
            }

            if (templateQuot.PreClosePrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.PreClosePrice), templateQuot.PreClosePrice)
                        ?? templateQuot.PreClosePrice;
                wait2UpdateQuot.PreClosePrice = destValue;
            }

            if (templateQuot.Turnover != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.Turnover), templateQuot.Turnover)
                        ?? templateQuot.Turnover;
                wait2UpdateQuot.Turnover = destValue;
            }

            if (templateQuot.UpperLimitPrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.UpperLimitPrice), templateQuot.UpperLimitPrice)
                        ?? templateQuot.UpperLimitPrice;
                wait2UpdateQuot.UpperLimitPrice = destValue;
            }

            if (templateQuot.LowerLimitPrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.LowerLimitPrice), templateQuot.LowerLimitPrice)
                        ?? templateQuot.LowerLimitPrice;
                wait2UpdateQuot.LowerLimitPrice = destValue;
            }

            if (templateQuot.AveragePrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.AveragePrice), templateQuot.AveragePrice)
                        ?? templateQuot.AveragePrice;
                wait2UpdateQuot.AveragePrice = destValue;
            }

            // 设置 IncreasePrice
            NativeQuotationItem.CalculateIncreasePrice(wait2UpdateQuot.LastPrice, wait2UpdateQuot.PreSettlementPrice,
                out double? increasePrice, out double? increasePriceRate);
            if (increasePrice != null)
            {
                var destValue = pricePropertyValueConverter?.Invoke(nameof(NativeQuotationItem.IncreasePrice), increasePrice)
                        ?? increasePrice;
                wait2UpdateQuot.IncreasePrice = destValue;
            }
        }
    }
}
