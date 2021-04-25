using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class EditComposeNameDialogContentViewModel : ViewModel<IEditComposeNameDialogContentView>
    {
        [ImportingConstructor]
        public EditComposeNameDialogContentViewModel(IEditComposeNameDialogContentView view) : base(view)
        {
        }
        
        private UserComposeViewContainer editComposeViewContainer;
        /// <summary>
        /// 要修改名称的组合视图容器
        /// </summary>
        public UserComposeViewContainer EditComposeViewContainer
        {
            get { return editComposeViewContainer; }
            set { SetProperty(ref editComposeViewContainer, value); }
        }

        private string newAliasName;
        public string NewAliasName
        {
            get { return newAliasName; }
            set { SetProperty(ref newAliasName, value); }
        }

        private ICommand okCommand;
        public ICommand OkCommand
        {
            get { return okCommand; }
            set { SetProperty(ref okCommand, value); }
        }
        
        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { SetProperty(ref cancelCommand, value); }
        }
    }
}
