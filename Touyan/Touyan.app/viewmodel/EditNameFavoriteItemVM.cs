using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using Touyan.app.view;

namespace Touyan.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class EditNameFavoriteItemVM : ViewModel<EditNameFavoriteItemView>
    {
        [ImportingConstructor]
        public EditNameFavoriteItemVM(EditNameFavoriteItemView view) : base(view)
        {
        }

        private bool showOriginNameRow;
        public bool ShowOriginNameRow
        {
            get { return showOriginNameRow; }
            set { SetProperty(ref showOriginNameRow, value); }
        }

        private string favoriteItemOriginName;
        public string FavoriteItemOriginName
        {
            get { return favoriteItemOriginName; }
            set { SetProperty(ref favoriteItemOriginName, value); }
        }

        private string favoriteItemNewName;
        public string FavoriteItemNewName
        {
            get { return favoriteItemNewName; }
            set { SetProperty(ref favoriteItemNewName, value); }
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
