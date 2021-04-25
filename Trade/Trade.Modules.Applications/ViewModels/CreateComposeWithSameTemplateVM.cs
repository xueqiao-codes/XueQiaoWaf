using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class CreateComposeWithSameTemplateVM : ViewModel<ICreateComposeWithSameTemplateView>
    {
        [ImportingConstructor]
        public CreateComposeWithSameTemplateVM(ICreateComposeWithSameTemplateView view) : base(view)
        {
        }
        
        private TargetCompose_ComposeDetail templateComposeDetailContainer;
        public TargetCompose_ComposeDetail TemplateComposeDetailContainer
        {
            get { return templateComposeDetailContainer; }
            set { SetProperty(ref templateComposeDetailContainer, value); }
        }
        
        private ICommand createWithTemplateCmd;
        public ICommand CreateWithTemplateCmd
        {
            get { return createWithTemplateCmd; }
            set { SetProperty(ref createWithTemplateCmd, value); }
        }
        
        private bool isCreating;
        public bool IsCreating
        {
            get { return isCreating; }
            set
            {
                if (SetProperty(ref isCreating, value))
                {
                    IsCreateButtonEnabled = !isCreating;
                }
            }
        }

        private bool isCreateButtonEnabled = true;
        public bool IsCreateButtonEnabled
        {
            get { return isCreateButtonEnabled; }
            private set { SetProperty(ref isCreateButtonEnabled, value); }
        }
    }
}
