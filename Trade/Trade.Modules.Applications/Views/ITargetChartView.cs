using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface ITargetChartView : IView
    {
        /// <summary>
        /// 添加图表行情
        /// </summary>
        /// <param name="resetChartFirst">是否先重置图表</param>
        /// <param name="quotations">新加的行情</param>
        void AddChartQuotations(bool resetChartFirst, IEnumerable<NativeQuotationItem> quotations);

        /// <summary>
        /// 重置图表
        /// </summary>
        void ResetChart();

        /// <summary>
        /// Shutdown view
        /// </summary>
        void Shutdown();
    }
}
