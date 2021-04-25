using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Touyan.app.datamodel
{
    public class ChartFavoriteListTreeNodeBase : Model
    {
        public ChartFavoriteListTreeNodeBase(long favoriteId)
        {
            this.FavoriteId = favoriteId;
        }

        public long FavoriteId { get; private set; }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { SetProperty(ref isExpanded, value); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }
    }
}
