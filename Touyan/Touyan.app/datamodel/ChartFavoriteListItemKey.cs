using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.datamodel;

namespace Touyan.app.datamodel
{
    public class ChartFavoriteListItemKey : Tuple<ChartFolderListItemType, long>
    {
        public ChartFavoriteListItemKey(ChartFolderListItemType itemType, long favoriteId) : base(itemType, favoriteId)
        {
            this.ItemType = itemType;
            this.FavoriteId = favoriteId;
        }

        public readonly ChartFolderListItemType ItemType;
        public readonly long FavoriteId;
    }
}
