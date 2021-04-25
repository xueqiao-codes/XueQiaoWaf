using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    /// <summary>
    /// 雪橇下单剧本验证辅助
    /// </summary>
    public static class XQPlaceOrderDramaValidateHelper
    {
        /// <summary>
        /// 验证 LimitPrice 类型的下单剧本
        /// </summary>
        /// <param name="drama">剧本</param>
        /// <param name="validateErrMsg">验证不通过的错误信息</param>
        /// <returns>是否验证通过</returns>
        public static bool ValidatePlaceOrderDrama_LimitPrice(PlaceOrderViewCreateDramaBase drama, out string validateErrMsg)
        {
            validateErrMsg = null;
            if (drama == null) throw new ArgumentNullException("drama");

            var orderPriceTypeValue = drama.SelectedPriceTypeValue;
            if (orderPriceTypeValue == null)
            {
                validateErrMsg = "请选择订单价格类型";
                return false;
            }
            
            var effectDateTypeValue = drama.SelectedEffectDateTypeValue;
            if (effectDateTypeValue == null)
            {
                validateErrMsg = "请选择有效时间类型";
                return false;
            }

            if (effectDateTypeValue.Type == HostingXQEffectDateType.XQ_EFFECT_PERIOD)
            {
                if (effectDateTypeValue.EndEffectTimestampS <= 0)
                {
                    validateErrMsg = "请选择有效时间的结束时间";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 验证 Parked 类型的下单剧本
        /// </summary>
        /// <param name="drama">剧本</param>
        /// <param name="validateErrMsg">验证不通过的错误信息</param>
        /// <returns>是否验证通过</returns>
        public static bool ValidatePlaceOrderDrama_Parked(PlaceOrderViewCreateDramaBase drama, out string validateErrMsg)
        {
            validateErrMsg = null;
            if (drama == null) throw new ArgumentNullException("drama");

            var orderPriceTypeValue = drama.SelectedPriceTypeValue;
            if (orderPriceTypeValue == null)
            {
                validateErrMsg = "请选择订单价格类型";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证 Condition 类型的下单剧本
        /// </summary>
        /// <param name="drama">剧本</param>
        /// <param name="validateErrMsg">验证不通过的错误信息</param>
        /// <returns>是否验证通过</returns>
        public static bool ValidatePlaceOrderDrama_Condition(PlaceOrderViewCreateDramaBase drama, out string validateErrMsg)
        {
            validateErrMsg = null;
            if (drama == null) throw new ArgumentNullException("drama");

            var effectDateTypeValue = drama.SelectedEffectDateTypeValue;
            if (effectDateTypeValue == null)
            {
                validateErrMsg = "请选择有效时间类型";
                return false;
            }

            if (effectDateTypeValue.Type == HostingXQEffectDateType.XQ_EFFECT_PERIOD)
            {
                if (effectDateTypeValue.EndEffectTimestampS <= 0)
                {
                    validateErrMsg = "请选择有效时间的结束时间";
                    return false;
                }
            }

            var triggerOrderPriceTypeValue = drama.SelectedPriceTypeValue;
            if (triggerOrderPriceTypeValue == null)
            {
                validateErrMsg = "请选择触发订单价格类型";
                return false;
            }

            return true;
        }
    }
}
