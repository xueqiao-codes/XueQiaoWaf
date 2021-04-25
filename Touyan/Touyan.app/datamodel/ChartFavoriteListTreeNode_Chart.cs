using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.datamodel;

namespace Touyan.app.datamodel
{
    public class ChartFavoriteListTreeNode_Chart : ChartFavoriteListTreeNodeBase
    {
        public ChartFavoriteListTreeNode_Chart(long favoriteId, long chartId) : base(favoriteId)
        {
            this.ChartId = chartId;
        }

        public long ChartId { get; private set; }

        private ChartFavoriteListItem_Chart favoriteData;
        public ChartFavoriteListItem_Chart FavoriteData
        {
            get { return favoriteData; }
            set { SetProperty(ref favoriteData, value); }
        }
    }
}
