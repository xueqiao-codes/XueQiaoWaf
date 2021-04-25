using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;

namespace XueQiaoWaf.Trade.Modules.Presentation.Converters
{
    /// <summary>
    /// 雪橇组合腿执行参数描述转换器
    /// </summary>
    public class XqComposeOrderLegExecParamsDM2DescConverter : IValueConverter
    {
        /// <summary>
        /// 描述条目间的分割符
        /// </summary>
        public string DescItemSeperator { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var legExecParamsItem = value as XqComposeOrderLegExecParamsDM;
            if (legExecParamsItem == null) return null;
            var orderSendType = legExecParamsItem.OrderSendType;
            if (orderSendType == XQComposeOrderExecParamsSendType.PARALLEL_LEG)
                return FormatDescForParallelTypeParams(legExecParamsItem.ParallelTypeParams);
            else if (orderSendType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_BEST)
                return FormatDescForSerialLegPriceBestTypeParams(legExecParamsItem.SerialLegPriceBestTypeParams);
            else if (orderSendType == XQComposeOrderExecParamsSendType.SERIAL_LEG_PRICE_TRYING)
                return FormatDescForSerialLegPriceTryingTypeParams(legExecParamsItem.SerialLegPriceTryingTypeParams);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        private string FormatDescForParallelTypeParams(XqComposeOrderLegParams_Parallel typeParams)
        {
            var item1 = "";
            if (typeParams.SendOrderExtraParam != null && typeParams.SendOrderExtraParam.__isset.quantityRatio)
            {
                item1 = $"挂单量不超过当前盘口量的 {XQComposeOrderEPTHelper.Convert2ClientQuantityRatio(typeParams.SendOrderExtraParam.QuantityRatio) * 100} %";
            }

            var item2 = "";
            var item3 = "";
            var item4 = "";
            if (typeParams.ChaseParam != null)
            {
                if (typeParams.ChaseParam.__isset.ticks)
                {
                    item2 = $"按对手价 加 {typeParams.ChaseParam.Ticks} 个（Tick）追";
                }
                if (typeParams.ChaseParam.__isset.protectPriceRatio)
                {
                    item3 = $"追价价格不超过初次触发价格的 {typeParams.ChaseParam.ProtectPriceRatio*1000} ‰（千分之）";
                }
                if (typeParams.ChaseParam.__isset.times)
                {
                    item4 = $"触发后撤单次数不超过 {typeParams.ChaseParam.Times} 次";
                }
            }

            var components = new string[] { item1, item2, item3, item4 };
            return FormatDescWithComponents(components);
        }

        private string[] GetFormatDescComponentsForSerialLegTypeParamsBase(XqComposeOrderLegParams_SerialLeg typeParams)
        {
            var item1 = "";
            if (typeParams.SendOrderExtraParam != null && typeParams.SendOrderExtraParam.__isset.quantityRatio)
            {
                item1 = $"挂单量不超过当前盘口量的 {XQComposeOrderEPTHelper.Convert2ClientQuantityRatio(typeParams.SendOrderExtraParam.QuantityRatio) * 100} %";
            }

            var item2 = "";
            if (typeParams.FirstLegExtraParam != null && typeParams.FirstLegExtraParam.__isset.revokeDeviatePriceTicks)
            {
                item2 = $"作为先手时，目标价和已挂单价偏离 {typeParams.FirstLegExtraParam.RevokeDeviatePriceTicks} 个价位（Tick）即撤单";
            }

            var item3 = "";
            var item4 = "";
            var item5 = "";
            if (typeParams.ChaseParam != null)
            {
                if (typeParams.ChaseParam.__isset.ticks)
                    item3 = $"作为后手时，按对手价 加 {typeParams.ChaseParam.Ticks} 个价位（Tick）追";
                if (typeParams.ChaseParam.__isset.protectPriceRatio)
                    item4 = $"追价价格不超过初次触发价格的 {typeParams.ChaseParam.ProtectPriceRatio*1000} ‰（千分之）";
                if (typeParams.ChaseParam.__isset.times)
                    item5 = $"触发后撤单次数不超过 {typeParams.ChaseParam.Times} 次";
            }
            return new string[] { item1, item2, item3, item4, item5 };
        }

        private string FormatDescForSerialLegPriceBestTypeParams(XqComposeOrderLegParams_SerialLeg typeParams)
        {
            var components = new List<string>();
            components.AddRange(GetFormatDescComponentsForSerialLegTypeParamsBase(typeParams));
            return FormatDescWithComponents(components);
        }

        private string FormatDescForSerialLegPriceTryingTypeParams(XqComposeOrderLegParams_SerialPriceTrying typeParams)
        {
            string item1 = "";
            if (typeParams.FirstLegTryingParam != null && typeParams.FirstLegTryingParam.__isset.beyondInPriceTicks)
            {
                item1 = $"作为先手时，发单价格优于买一价（卖一价）{typeParams.FirstLegTryingParam.BeyondInPriceTicks} 个价位（Tick）开始触发";
            }
            var components = new List<string>();
            components.Add(item1);
            components.AddRange(GetFormatDescComponentsForSerialLegTypeParamsBase(typeParams));
            return FormatDescWithComponents(components);
        }

        private string FormatDescWithComponents(IEnumerable<string> descComponents)
        {
            if (descComponents == null) return null;
            descComponents = descComponents.Where(i => !string.IsNullOrEmpty(i)).ToArray();
            return string.Join(this.DescItemSeperator ?? "", descComponents);
        }
    }
}
