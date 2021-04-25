using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using Touyan.app.view;
using xueqiao.graph.xiaoha.chart.thriftapi;
using XueQiaoFoundation.UI.Extra.helper;

namespace Touyan.app.viewmodel
{
    public delegate Chart[] ChartSearchSuggestionGetter(string filter);

    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ChartDetailContainerVM : ViewModel<ChartDetailContainerView>
    {
        [ImportingConstructor]
        public ChartDetailContainerVM(ChartDetailContainerView view) : base(view)
        {
            ChartSearchSuggestionProvider = new CollectionViewSuggestionProvider(_filter => 
            {
                var charts = this.ChartSearchSuggestionGetter?.Invoke(_filter);
                if (charts == null) charts = new Chart[] { };
                
                var collectionView = CollectionViewSource.GetDefaultView(charts);
                collectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Chart.ParentFolderId)));

                return collectionView;
            });
        }
        
        /// <summary>
        /// 使用 url 方式加载图表内容
        /// </summary>
        /// <param name="url"></param>
        public void LoadChartWebContentWithUrl(string url)
        {
            ViewCore.LoadChartWebContentWithUrl(url);
        }

        /// <summary>
        /// 使用 html 内容方式加载图表内容
        /// </summary>
        /// <param name="htmlContent"></param>
        public void LoadChartWebContentWithHtmlContent(string htmlContent)
        {
            ViewCore.LoadChartWebContentWithHtmlContent(htmlContent);
        }

        /// <summary>
        /// 图表是否被收藏
        /// </summary>
        private bool chartIsFavorited;
        public bool ChartIsFavorited
        {
            get { return chartIsFavorited; }
            set { SetProperty(ref chartIsFavorited, value); }
        }

        /// <summary>
        /// 图表收藏或取消 command
        /// </summary>
        private ICommand toggleChartFavoriteCmd;
        public ICommand ToggleChartFavoriteCmd
        {
            get { return toggleChartFavoriteCmd; }
            set { SetProperty(ref toggleChartFavoriteCmd, value); }
        }

        /// <summary>
        /// 是否正在加载页面数据
        /// </summary>
        private bool isLoadingPageData;
        public bool IsLoadingPageData
        {
            get { return isLoadingPageData; }
            set { SetProperty(ref isLoadingPageData, value); }
        }
        
        public CollectionViewSuggestionProvider ChartSearchSuggestionProvider { get; private set; }

        public ChartSearchSuggestionGetter ChartSearchSuggestionGetter { get; set; }

        private Chart selectedSuggestionChart;
        public Chart SelectedSuggestionChart
        {
            get { return selectedSuggestionChart; }
            set { SetProperty(ref selectedSuggestionChart, value); }
        }
    }
}
