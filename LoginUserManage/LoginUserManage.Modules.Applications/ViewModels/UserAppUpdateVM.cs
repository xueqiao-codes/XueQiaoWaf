using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.LoginUserManage.Modules.Applications.Views;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserAppUpdateVM : ViewModel<IUserAppUpdateView>
    {
        [ImportingConstructor]
        public UserAppUpdateVM(IUserAppUpdateView view) : base(view)
        {
            DownloadProgressDataContext = new UserAppDowloadProgressDataContext();
        }

        public UserAppDowloadProgressDataContext DownloadProgressDataContext { get; private set; }

        private string appUpdateDesc;
        public string AppUpdateDesc
        {
            get { return appUpdateDesc; }
            set { SetProperty(ref appUpdateDesc, value); }
        }

        private bool isForceUpdate;
        public bool IsForceUpdate
        {
            get { return isForceUpdate; }
            set { SetProperty(ref isForceUpdate, value); }
        }

        private ICommand updateNowCmd;
        public ICommand UpdateNowCmd
        {
            get { return updateNowCmd; }
            set { SetProperty(ref updateNowCmd, value); }
        }

        private ICommand cancelUpdateCmd;
        public ICommand CancelUpdateCmd
        {
            get { return cancelUpdateCmd; }
            set { SetProperty(ref cancelUpdateCmd, value); }
        }
    }
}
