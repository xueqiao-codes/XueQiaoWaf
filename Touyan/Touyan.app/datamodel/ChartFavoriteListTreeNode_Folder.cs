using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.datamodel;

namespace Touyan.app.datamodel
{
    public class ChartFavoriteListTreeNode_Folder : ChartFavoriteListTreeNodeBase
    {
        public ChartFavoriteListTreeNode_Folder(long folderId) : base(folderId)
        {
            this.FolderId = folderId;
            Children = new ObservableCollection<ChartFavoriteListTreeNodeBase>();
        }

        public long FolderId { get; private set; }

        public ObservableCollection<ChartFavoriteListTreeNodeBase> Children { get; private set; }
        
        private ChartFavoriteListItem_Folder favoriteData;
        public ChartFavoriteListItem_Folder FavoriteData
        {
            get { return favoriteData; }
            set { SetProperty(ref favoriteData, value); }
        }
    }
}
