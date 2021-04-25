using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ComponentTabListSelectionViewModel : ViewModel<IComponentTabListSelectionView>
    {
        [ImportingConstructor]
        public ComponentTabListSelectionViewModel(
            IComponentTabListSelectionView view) : base(view)
        {
            ComponentTypes = new ObservableCollection<ComponentTabSelectionItem>();
        }
        
        private ICommand checkCommand;  
        public ICommand CheckCommand
        {
            get { return checkCommand; }
            set { SetProperty(ref checkCommand, value); }
        }

        private ICommand uncheckCommand;
        public ICommand UncheckCommand
        {
            get { return uncheckCommand; }
            set { SetProperty(ref uncheckCommand, value); }
        }

        public ObservableCollection<ComponentTabSelectionItem> ComponentTypes { get; private set; }

        public void ShowPopup(object targetElement)
        {
            ViewCore.ShowPopup(targetElement);
        }

        public void Close()
        {
            ViewCore.Close();
        }
    }
}
