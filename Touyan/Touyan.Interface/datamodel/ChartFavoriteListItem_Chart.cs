using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.thriftapi;
using xueqiao.personal.user.thriftapi;

namespace Touyan.Interface.datamodel
{
    public class ChartFavoriteListItem_Chart : ChartFavoriteListItem
    {
        public ChartFavoriteListItem_Chart(long favoriteId, long chartId) : base(ChartFolderListItemType.Chart, favoriteId)
        {
            this.ChartId = chartId;
        }
        
        public long ChartId { get; private set; }
        
        private FavoriteChart favoriteInfo;
        public FavoriteChart FavoriteInfo
        {
            get { return favoriteInfo; }
            set { SetProperty(ref favoriteInfo, value); }
        }

        private Chart chartInfo;
        public Chart ChartInfo
        {
            get { return chartInfo; }
            set { SetProperty(ref chartInfo, value); }
        }
    }
}
