using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using Touyan.app.datamodel;
using Touyan.app.view;

namespace Touyan.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ChartFavoriateListVM : ViewModel<ChartFavoriateListView>
    {
        [ImportingConstructor]
        public ChartFavoriateListVM(ChartFavoriateListView view) : base(view)
        {

        }

        private bool showLink2TouyanUserGuideView;
        /// <summary>
        /// 是否显示关联至投研用户引导视图
        /// </summary>
        public bool ShowLink2TouyanUserGuideView
        {
            get { return showLink2TouyanUserGuideView; }
            set { SetProperty(ref showLink2TouyanUserGuideView, value); }
        }

        private bool authorUserHasLogined;
        /// <summary>
        /// 授权用户是否已登录
        /// </summary>
        public bool AuthorUserHasLogined
        {
            get { return authorUserHasLogined; }
            set { SetProperty(ref authorUserHasLogined, value); }
        }

        private bool hasUserRegisterFeature;
        /// <summary>
        /// 是否有注册功能。当<see cref="AuthorUserHasLogined"/> 为 false 时，如果 <see cref="HasUserRegisterFeature"/> 为 true，则会显示注册入口
        /// </summary>
        public bool HasUserRegisterFeature
        {
            get { return hasUserRegisterFeature; }
            set { SetProperty(ref hasUserRegisterFeature, value); }
        }

        private ICommand loginEntryCmd;
        public ICommand LoginEntryCmd
        {
            get { return loginEntryCmd; }
            set { SetProperty(ref loginEntryCmd, value); }
        }

        private ICommand registerEntryCmd;
        public ICommand RegisterEntryCmd
        {
            get { return registerEntryCmd; }
            set { SetProperty(ref registerEntryCmd, value); }
        }

        private ICommand link2TouyanUserEntryCmd;
        public ICommand Link2TouyanUserEntryCmd
        {
            get { return link2TouyanUserEntryCmd; }
            set { SetProperty(ref link2TouyanUserEntryCmd, value); }
        }
        
        private ChartFavoriteListTreeNode_Folder favoriteListRootFolderNode;
        /// <summary>
        /// 收藏列表根文件夹 node
        /// </summary>
        public ChartFavoriteListTreeNode_Folder FavoriteListRootFolderNode
        {
            get { return favoriteListRootFolderNode; }
            set { SetProperty(ref favoriteListRootFolderNode, value); }
        }
        
        private ICommand refreshFavoriteListCmd;
        public ICommand RefreshFavoriteListCmd
        {
            get { return refreshFavoriteListCmd; }
            set { SetProperty(ref refreshFavoriteListCmd, value); }
        }

        private ICommand newTopLevelFolderCmd;
        public ICommand NewTopLevelFolderCmd
        {
            get { return newTopLevelFolderCmd; }
            set { SetProperty(ref newTopLevelFolderCmd, value); }
        }

        private ICommand newChildFolderCmd;
        public ICommand NewChildFolderCmd
        {
            get { return newChildFolderCmd; }
            set { SetProperty(ref newChildFolderCmd, value); }
        }

        private ICommand moveFavoriteItemCmd;
        public ICommand MoveFavoriteItemCmd
        {
            get { return moveFavoriteItemCmd; }
            set { SetProperty(ref moveFavoriteItemCmd, value); }
        }

        private ICommand renameFavoriteItemCmd;
        public ICommand RenameFavoriteItemCmd
        {
            get { return renameFavoriteItemCmd; }
            set { SetProperty(ref renameFavoriteItemCmd, value); }
        }

        private ICommand removeFavoriteItemCmd;
        public ICommand RemoveFavoriteItemCmd
        {
            get { return removeFavoriteItemCmd; }
            set { SetProperty(ref removeFavoriteItemCmd, value); }
        }

    }
}
