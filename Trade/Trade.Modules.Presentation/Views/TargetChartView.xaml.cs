//using SciChart.Charting.ChartModifiers;
//using SciChart.Charting.Model.DataSeries;
//using SciChart.Charting.Numerics.Calendars;
//using SciChart.Charting.ViewportManagers;
//using SciChart.Charting.Visuals;
//using SciChart.Charting.Visuals.Axes;
//using SciChart.Charting.Visuals.Axes.DiscontinuousAxis;
//using SciChart.Charting.Visuals.Axes.LabelProviders;
//using SciChart.Charting.Visuals.RenderableSeries;
//using SciChart.Charting.Visuals.TradeChart;
//using SciChart.Data.Model;
//using SciChart.Core.Utility.Mouse;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;
using System.ComponentModel;
using XueQiaoFoundation.Shared.Interface;
using NativeModel.Trade;

namespace XueQiaoWaf.Trade.Modules.Presentation.Views
{
    /// <summary>
    /// TargetChartView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ITargetChartView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TargetChartView : ITargetChartView, IShutdownObject
    {
        public TargetChartView()
        {
            InitializeComponent();
            // 了解 Loaded 事件的触发时机，确保只在第一次 Loaded 事件发生时进行图表设置，避免图表重置
            Loaded += ViewFirstLoadedHandler;
        }

        private void ViewFirstLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= ViewFirstLoadedHandler;
            SetupChartView();
        }

        public void AddChartQuotations(bool resetChartFirst, IEnumerable<NativeQuotationItem> quotations)
        { 
            // do nothing
        }

        public void ResetChart()
        {
            // do nothing
        }

        public void Shutdown()
        {
            // do nothing
        }

        private void SetupChartView()
        {
            //LoadSciTickCharts();
            SetupChartNotOpenTipView();
        }

        private void SetupChartNotOpenTipView()
        {
            var textBlock = new TextBlock {
                Text = "图表暂未开放，敬请期待！",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 20};

            this.ChartGrid.Children.Clear();
            this.ChartGrid.Children.Add(textBlock);
        }

        /*
        private class TickPointMeta : IPointMetadata
        {
            public TickPointMeta(double? increasePrice, double? increasePriceRate)
            {
                this.IncreasePrice = increasePrice;
                this.IncreasePriceRate = increasePriceRate;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public bool IsSelected { get; set; }

            public double? IncreasePrice { get; private set; }
            public double? IncreasePriceRate { get; private set; }
        }

        private class TickChartViewportManager : DefaultViewportManager
        {
            private readonly DateTime? minimunDateLimit;

            public TickChartViewportManager(DateTime? minimunDateLimit)
            {
                this.minimunDateLimit = minimunDateLimit;
            }

            protected override IRange OnCalculateNewXRange(IAxis xAxis)
            {
                var range = (DateRange)base.OnCalculateNewXRange(xAxis);

                // Do some manipulation to the range
                if (minimunDateLimit != null)
                {
                    if (range.Min < minimunDateLimit.Value)
                    {
                        var span = minimunDateLimit.Value - range.Min;
                        range = new DateRange(minimunDateLimit.Value, range.Max + span);
                    }
                }

                return range;
            }
        }

        private class TickDateTimeAxisLabelProvider : DiscontinuousDateTimeLabelProvider
        {
            public override string FormatLabel(IComparable dataValue)
            {
                return ((DateTime)dataValue).ToString("HH:mm");
            }
        }

        ///<summary>
        /// Example of how to make a Discontinuous DateTime Calender for the London Stock Exchange
        /// 
        /// If you wish to extend this, ensure that public holidays are set for the year(s) which you wish to show data
        /// e.g. http://www.lseg.com/areas-expertise/our-markets/london-stock-exchange/equities-markets/trading-services/business-days
        ///</summary>
        private class LSECalendar : DiscontinuousDateTimeCalendarBase
        {
            public LSECalendar()
            {
                // For intraday data, you can add a skip range like this.
                // For daily data, skip ranges will cause the Daily OHLC bars with timestamp at 0:00:00 to be skipped 
                SkipDayTimeRange.Add(new TimeSpanRange(new TimeSpan(0, 0, 0), new TimeSpan(8, 0, 0))); // LSE is open at 08:00am GMT
                SkipDayTimeRange.Add(new TimeSpanRange(new TimeSpan(16, 30, 0), new TimeSpan(24, 0, 0))); // LSE is closed at 16:30pm GMT

                // LSE is closed on weekends
                SkipDaysInWeek.Add(DayOfWeek.Saturday);
                SkipDaysInWeek.Add(DayOfWeek.Sunday);

                SkipDates.Add(new DateTime(2015, 12, 25)); // LSE Closed on Christmas Day 2015
                SkipDates.Add(new DateTime(2016, 1, 1)); // LSE Closed on New Years day 2016
            }
        }

        private class TickChartZoomExtentsModifier : ZoomExtentsModifier
        {
            public TickChartZoomExtentsModifier()
            {
            }
            
            public override void OnModifierDoubleClick(ModifierMouseArgs e)
            {
                var handled = false;
                if (ExecuteOn == ExecuteOn.MouseDoubleClick)
                {
                    var chartSurface = this.ParentSurface;
                    if (chartSurface != null)
                    {
                        var xDateTimeAxis = chartSurface.XAxes.FirstOrDefault() as DiscontinuousDateTimeAxis;
                        if (xDateTimeAxis != null)
                        {
                            var dataRange = xDateTimeAxis.DataRange as DateRange;
                            if (dataRange != null)
                            {
                                var newVisibleRange = new DateRange(dataRange.Min, dataRange.Max + new TimeSpan(1, 0, 0));
                                newVisibleRange = newVisibleRange.GrowBy(xDateTimeAxis.GrowBy.Min, xDateTimeAxis.GrowBy.Max) as DateRange;
                                xDateTimeAxis.VisibleRange = newVisibleRange;
                                var animDur = new TimeSpan(0, 0, 0, 0, 100);
                                xDateTimeAxis.AnimateVisibleRangeTo(newVisibleRange, animDur);
                                chartSurface.AnimateZoomExtentsY(animDur);
                                handled = true;
                            }
                        }
                    }
                }

                if (!handled)
                {
                    base.OnModifierDoubleClick(e);
                }
            }
        }

        private SciChartSurface sciTickPriceChartSurface;
        private FastLineRenderableSeries lastPriceLineRenderableSeries;
        private SciChartSurface sciTickVolumeChartSurface;
        private FastLineRenderableSeries volumeLineRenderableSeries;
        
        private void LoadSciTickCharts()
        {
            // set sciTickPriceChartSurface xAxis
            var nowDateTime = DateTime.Now; 
            var todayDate = nowDateTime.Date;

            // set sciTickPriceChartSurface
            sciTickPriceChartSurface = new SciChartSurface();
            sciTickPriceChartSurface.ViewportManager = new TickChartViewportManager(todayDate);
            
            var priceChartXAxis = new DiscontinuousDateTimeAxis
            {
                DrawLabels = false,
                MajorDelta = new TimeSpan(1, 0, 0),
                MinorDelta = new TimeSpan(0,12,0),
                MinimalZoomConstrain = new TimeSpan(1,0,0),
                AutoTicks = false,
                DrawMajorGridLines = true,
                DrawMinorGridLines = false,
                AutoRange = AutoRange.Never,
                LabelProvider = new TickDateTimeAxisLabelProvider(),
                //Calendar = new LSECalendar(), 
                VisibleRange = new DateRange(nowDateTime - new TimeSpan(1,0,0), nowDateTime + new TimeSpan(3,0,0)),
            };

            sciTickPriceChartSurface.XAxes.Add(priceChartXAxis);

            // set sciTickPriceChartSurface yAxis
            var priceYAxis = new NumericAxis
            {
                DrawMajorGridLines = true,
                DrawMinorGridLines = false,
                AutoRange = AutoRange.Always,
                GrowBy = new DoubleRange(0.2, 0.2)
            };
            sciTickPriceChartSurface.YAxes.Add(priceYAxis);

            // lastPriceLineSeries
            lastPriceLineRenderableSeries = new FastLineRenderableSeries
            {
                Stroke = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF),
                StrokeThickness = 1,
                DataSeries = new XyDataSeries<DateTime, double>(),
            };
            sciTickPriceChartSurface.RenderableSeries.Add(lastPriceLineRenderableSeries);
            CursorModifier.SetSnapToSeries(lastPriceLineRenderableSeries, true);
            
            // sciTickVolumeChartSurface
            sciTickVolumeChartSurface = new SciChartSurface();

            // set sciTickVolumeChartSurface yAxis
            var volumeYAxis = new NumericAxis
            {
                DrawMajorGridLines = true,
                DrawMinorGridLines = false,
                AutoRange = AutoRange.Always,
                GrowBy = new DoubleRange(0, 0)
            };
            sciTickVolumeChartSurface.YAxes.Add(volumeYAxis);
            
            var volumeChartXAxis = new DiscontinuousDateTimeAxis
            {
                DrawLabels = true,
                MajorDelta = new TimeSpan(1, 0, 0),
                MinorDelta = new TimeSpan(0, 12, 0),
                MinimalZoomConstrain = new TimeSpan(1, 0, 0),
                AutoTicks = false,
                DrawMajorGridLines = true,
                DrawMinorGridLines = false,
                AutoRange = AutoRange.Never,
                LabelProvider = new TickDateTimeAxisLabelProvider(),
                //Calendar = new LSECalendar()
            };
            sciTickVolumeChartSurface.XAxes.Add(volumeChartXAxis);

            // Bind the VisibleRange of chart2.XAxis.VisibleRange to chart1.XAxis.VisibleRange TwoWay
            var xAxisVisibleRangeBinding = new Binding("VisibleRange")
            {
                Source = priceChartXAxis,
                Mode = BindingMode.TwoWay,
            };
            BindingOperations.SetBinding(volumeChartXAxis, SciChart.Charting.Visuals.Axes.AxisBase.VisibleRangeProperty, xAxisVisibleRangeBinding);

            // volumeLineSeries
            volumeLineRenderableSeries = new FastLineRenderableSeries
            {
                Stroke = Color.FromArgb(0xFF, 0x0, 0xFF, 0xFF),
                StrokeThickness = 1,
                DataSeries = new XyDataSeries<DateTime, long>(),
            };
            sciTickVolumeChartSurface.RenderableSeries.Add(volumeLineRenderableSeries);
            CursorModifier.SetSnapToSeries(volumeLineRenderableSeries, true);

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var gridSplitter = new GridSplitter
            {
                MinHeight = 4,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x0))
            };

            Grid.SetRow(sciTickPriceChartSurface, 0);
            Grid.SetRow(gridSplitter, 1);
            Grid.SetRow(sciTickVolumeChartSurface, 2);

            grid.Children.Add(sciTickPriceChartSurface);
            grid.Children.Add(gridSplitter);
            grid.Children.Add(sciTickVolumeChartSurface);

            var priceChartSurfaceModifier = new ModifierGroup(

                // Allow pan on Left mouse drag
                new ZoomPanModifier { ExecuteOn = ExecuteOn.MouseLeftButton, ClipModeX = SciChart.Charting.ClipMode.None },
                // <!-- Allow Mousewheel Zoom -->
                new MouseWheelZoomModifier(),
                // <!-- Allow Zoom to Extents on double click -->
                new TickChartZoomExtentsModifier { ExecuteOn = ExecuteOn.MouseDoubleClick },
                // Alow cursor modifier
                new CursorModifier
                {
                    SourceMode = SourceMode.AllSeries,
                    ShowTooltipOn = ShowTooltipOptions.Always,
                    ShowAxisLabels = true,
                    ReceiveHandledEvents = true,
                    ShowTooltip = true,
                    IsEnabled = true,
                }
            );
            sciTickPriceChartSurface.ChartModifier = priceChartSurfaceModifier;

            var volumeChartSurfaceModifier = new ModifierGroup(
                
                // Allow pan on Left mouse drag
                new ZoomPanModifier { ExecuteOn = ExecuteOn.MouseLeftButton, ClipModeX = SciChart.Charting.ClipMode.None },
                // <!-- Allow Mousewheel Zoom -->
                new MouseWheelZoomModifier(),
                // <!-- Allow Zoom to Extents on double click -->
                new TickChartZoomExtentsModifier { ExecuteOn = ExecuteOn.MouseDoubleClick },
                // Alow cursor modifier
                new CursorModifier
                {
                    SourceMode = SourceMode.AllSeries,
                    ShowTooltipOn = ShowTooltipOptions.Always,
                    ShowAxisLabels = true,
                    ReceiveHandledEvents = true,
                    ShowTooltip = true,
                    IsEnabled = true,
                }
            );
            sciTickVolumeChartSurface.ChartModifier = volumeChartSurfaceModifier;

            var tickChartsMouseEventSharedGroupName = UUIDHelper.CreateUUIDString(false).ToString();
            MouseManager.SetMouseEventGroup(priceChartSurfaceModifier, tickChartsMouseEventSharedGroupName);
            MouseManager.SetMouseEventGroup(volumeChartSurfaceModifier, tickChartsMouseEventSharedGroupName);

            var tickChartSharedGroupName = UUIDHelper.CreateUUIDString(false).ToString();
            SciChartGroup.SetVerticalChartGroup(sciTickPriceChartSurface, tickChartSharedGroupName);
            SciChartGroup.SetVerticalChartGroup(sciTickVolumeChartSurface, tickChartSharedGroupName);

            this.ChartGrid.Children.Clear();
            this.ChartGrid.Children.Add(grid);
        }
        
        public void AddChartQuotations(bool resetChartFirst, IEnumerable<NativeQuotationItem> quotations)
        {
            if (isDisposed) return;

            if (quotations?.Any() != true) return;
            var __lastPriceDataSeries = this.lastPriceLineRenderableSeries?.DataSeries as XyDataSeries<DateTime, double>;
            var __volumeDataSeries = this.volumeLineRenderableSeries?.DataSeries as XyDataSeries<DateTime, long>;
            if (__lastPriceDataSeries != null
                && __volumeDataSeries != null)
            {
                using (__lastPriceDataSeries.SuspendUpdates())
                {
                    using (__volumeDataSeries.SuspendUpdates())
                    {
                        foreach (var _q in quotations)
                        {
                            var dateTime = DateHelper.UnixTimeStampMsToDateTime(_q.UpdateTimestampMs).ToLocalTime();
                            if (_q.LastPrice != null)
                            {
                                __lastPriceDataSeries.Append(dateTime, _q.LastPrice.Value, new TickPointMeta(_q.IncreasePrice, _q.IncreasePriceRate));
                            }
                            if (_q.Volume != null)
                            {
                                __volumeDataSeries.Append(dateTime, _q.Volume.Value);
                            }
                        }
                    }
                }
            }
        }

        public void ResetChart()
        {
            if (isDisposed) return;

            ChartGrid.Children.Clear();

            if (sciTickPriceChartSurface != null)
            {
                // Chart's Dispose method needs to be called when chart is 
                // no longer needed so that all unmanaged resources 
                // (DirectX etc.) are released.
                sciTickPriceChartSurface?.ChartModifier?.OnDetached();
                sciTickPriceChartSurface?.Dispose();
                sciTickPriceChartSurface = null;
            }
            if (sciTickVolumeChartSurface != null)
            {
                // Chart's Dispose method needs to be called when chart is 
                // no longer needed so that all unmanaged resources 
                // (DirectX etc.) are released.
                sciTickVolumeChartSurface?.ChartModifier?.OnDetached();
                sciTickVolumeChartSurface?.Dispose();
                sciTickVolumeChartSurface = null;
            }
            LoadSciTickCharts();
        }

        ~TargetChartView()
        {
            Dispose(false);
        }

        #region IShutdownObject 

        private bool isDisposed = false;

        public void Shutdown()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected void Dispose(bool isDisposing)
        {
            if (isDisposed == true) return;
            if (isDisposing)
            {
                // Don't forget to clear chart grid child list.
                ChartGrid?.Children.Clear();

                if (sciTickPriceChartSurface != null)
                {
                    // Chart's Dispose method needs to be called when chart is 
                    // no longer needed so that all unmanaged resources 
                    // (DirectX etc.) are released.
                    sciTickPriceChartSurface.ChartModifier?.OnDetached();
                    sciTickPriceChartSurface?.Dispose();
                    sciTickPriceChartSurface = null;

                }
                if (sciTickVolumeChartSurface != null)
                {
                    // Chart's Dispose method needs to be called when chart is 
                    // no longer needed so that all unmanaged resources 
                    // (DirectX etc.) are released.
                    sciTickVolumeChartSurface?.ChartModifier?.OnDetached();
                    sciTickVolumeChartSurface?.Dispose();
                    sciTickVolumeChartSurface = null;
                }
            }

            isDisposed = true;
        }

        #endregion
        */
    }
}
