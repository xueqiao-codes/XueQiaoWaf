using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    
    public interface IPlaceOrderComponentContentView : IView
    {
        /// <summary>
        /// 下单 Column 的视图宽度
        /// </summary>
        double PlaceOrderColumnViewWidth { get; }

        /// <summary>
        /// 图表 Column 的视图宽度
        /// </summary>
        double ChartColumnViewWidth { get; }
    }
}
