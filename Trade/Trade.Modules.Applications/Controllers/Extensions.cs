using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.Helper;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    public static class Extensions
    {
        /// <summary>
        /// 计算下单组件的常规宽度
        /// </summary>
        /// <param name="showChartLayout">是否显示图表部分</param>
        /// <param name="showOrderLayout">是否显示下单部分</param>
        /// <returns></returns>
        public static int PlaceOrderComponent_NormalWidth(bool showChartLayout, bool showOrderLayout)
        {
            var layoutWidths = new List<int>();
            if (showChartLayout)
            {
                layoutWidths.Add(TradeComponentConstans.PlaceOrderComponent_ChartColumnNormalWidth);
            }
            if (showOrderLayout)
            {
                layoutWidths.Add(TradeComponentConstans.PlaceOrderComponent_PlaceOrderColumnNormalWidth);
            }

            // layouts widths
            var sumWidth = layoutWidths.Sum();
            
            // seperator width
            if (layoutWidths.Any())
            {
                sumWidth += ((layoutWidths.Count - 1) * TradeComponentConstans.PlaceOrderComponent_ColumnSeperaterWidth);
            }

            // border widths
            sumWidth += (2 * TradeComponentConstans.PlaceOrderComponent_BorderWidth);

            return sumWidth;
        }

        /// <summary>
        /// 计算下单组件的最小宽度
        /// </summary>
        /// <param name="showChartLayout">是否显示图表部分</param>
        /// <param name="showOrderLayout">是否显示下单部分</param>
        /// <returns></returns>
        public static int PlaceOrderComponent_MinimumWidth(bool showChartLayout, bool showOrderLayout)
        {
            var layoutWidths = new List<int>();
            if (showChartLayout)
            {
                layoutWidths.Add(TradeComponentConstans.PlaceOrderComponent_ChartColumnMininumWidth);
            }
            if (showOrderLayout)
            {
                layoutWidths.Add(TradeComponentConstans.PlaceOrderComponent_PlaceOrderColumnMininumWidth);
            }
            
            // layouts widths
            var sumWidth = layoutWidths.Sum();

            // seperator width
            if (layoutWidths.Any())
            {
                sumWidth += ((layoutWidths.Count - 1) * TradeComponentConstans.PlaceOrderComponent_ColumnSeperaterWidth);
            }

            // border widths
            sumWidth += (2 * TradeComponentConstans.PlaceOrderComponent_BorderWidth);

            return sumWidth;
        }
    }
}
