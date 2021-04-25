using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Touyan.Interface.datamodel
{
    public class ChartFavoriteListItem : Model
    {
        public ChartFavoriteListItem(ChartFolderListItemType itemType, long favoriteId)
        {
            this.ItemType = itemType;
            this.FavoriteId = favoriteId;
        }

        /// <summary>
        /// 列表项类型
        /// </summary>
        public ChartFolderListItemType ItemType { get; private set; }

        public long FavoriteId { get; private set; }
        
        private long parentFavoriteFolderId;
        /// <summary>
        /// 父收藏夹 id
        /// </summary>
        public long ParentFavoriteFolderId
        {
            get { return parentFavoriteFolderId; }
            set { SetProperty(ref parentFavoriteFolderId, value); }
        }
    }
}
