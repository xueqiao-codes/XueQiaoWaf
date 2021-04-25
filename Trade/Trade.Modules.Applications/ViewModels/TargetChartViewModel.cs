using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TargetChartViewModel : ViewModel<ITargetChartView>, IShutdownObject
    {
        public class DateTimeValue<T>
        {
            public DateTime DateTime { get; set; }
            public T Value { get; set; }
        }
        
        [ImportingConstructor]
        protected TargetChartViewModel(ITargetChartView view) : base(view)
        {
        }
        
        private readonly List<DateTimeValue<double>> tickPoints = new List<DateTimeValue<double>>();

        /// <summary>
        /// 添加图表行情
        /// </summary>
        /// <param name="resetChart">是否重置图表</param>
        /// <param name="quotations">新加的行情列表</param>
        public void AddChartQuotations(bool resetChart, IEnumerable<NativeQuotationItem> quotations)
        {
            ViewCore.AddChartQuotations(resetChart, quotations);
        }

        /// <summary>
        /// 重置图表
        /// </summary>
        public void ResetChart()
        {
            ViewCore.ResetChart();
        }

        #region IShutdownObject 

        public void Shutdown()
        {
            ViewCore.Shutdown();
        }

        #endregion
    }
}
