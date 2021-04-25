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
    public class MoveFavoriteItemVM : ViewModel<MoveFavoriteItemView>
    {
        [ImportingConstructor]
        public MoveFavoriteItemVM(MoveFavoriteItemView view) : base(view)
        {
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
