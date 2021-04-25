using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.personal.user.thriftapi;

namespace Touyan.Interface.datamodel
{
    public class ChartFavoriteListItem_Folder : ChartFavoriteListItem
    {
        public ChartFavoriteListItem_Folder(long folderId) : base(ChartFolderListItemType.Folder, folderId)
        {
            this.FolderId = folderId;
        }
        
        public long FolderId { get; private set; }

        private FavoriteFolder favoriteInfo;
        public FavoriteFolder FavoriteInfo
        {
            get { return favoriteInfo; }
            set { SetProperty(ref favoriteInfo, value); }
        }
    }
}
