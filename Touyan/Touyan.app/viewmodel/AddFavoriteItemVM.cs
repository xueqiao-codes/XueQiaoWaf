using System;
using System.Collections.Generic;
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
    public class AddFavoriteItemVM : ViewModel<AddFavoriteItemView>
    {
        [ImportingConstructor]
        public AddFavoriteItemVM(AddFavoriteItemView view) : base(view)
        {
        }

        private string favoriteItemName;
        public string FavoriteItemName
        {
            get { return favoriteItemName; }
            set { SetProperty(ref favoriteItemName, value); }
        }

        private bool hiddenFavorFolderSelectionView;
        /// <summary>
        /// 是否隐藏收藏文件夹选择视图
        /// </summary>
        public bool HiddenFavorFolderSelectionView
        {
            get { return hiddenFavorFolderSelectionView; }
            set { SetProperty(ref hiddenFavorFolderSelectionView, value); }
        }

        private double viewWidth;
        /// <summary>
        /// 设置视图的宽度
        /// </summary>
        public double ViewWidth
        {
            get { return viewWidth; }
            set { SetProperty(ref viewWidth, value); }
        }
        
        private ChartFavoriteNodeTree favorFolderTree;
        public ChartFavoriteNodeTree FavorFolderTree
        {
            get { return favorFolderTree; }
            set { SetProperty(ref favorFolderTree, value); }
        }

        private ICommand submitCmd;
        public ICommand SubmitCmd
        {
            get { return submitCmd; }
            set { SetProperty(ref submitCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }
    }
}
