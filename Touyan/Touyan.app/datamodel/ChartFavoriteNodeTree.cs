using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Touyan.app.datamodel
{
    /// <summary>
    /// 投研图表收藏树 data model
    /// </summary>
    public class ChartFavoriteNodeTree : Model
    {
        public ChartFavoriteNodeTree()
        {
            NodeList = new ObservableCollection<ChartFavoriteListTreeNodeBase>();
        }

        public ObservableCollection<ChartFavoriteListTreeNodeBase> NodeList { get; private set; }
    }
}
