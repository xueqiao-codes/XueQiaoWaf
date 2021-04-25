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
    public class UserAppVersionInfoVM : ViewModel<IUserAppVersionInfoView>
    {
        [ImportingConstructor]
        public UserAppVersionInfoVM(IUserAppVersionInfoView view) : base(view)
        {
            DownloadProgressDataContext = new UserAppDowloadProgressDataContext();
        }
        
        public UserAppDowloadProgressDataContext DownloadProgressDataContext { get; private set; }

        private string currentVersion;
        public string CurrentVersion
        {
            get { return currentVersion; }
            set { SetProperty(ref currentVersion, value); }
        }

        private string newVersion;
        public string NewVersion
        {
            get { return newVersion; }
            set { SetProperty(ref newVersion, value); }
        }

        private bool hasNewVersion;
        public bool HasNewVersion
        {
            get { return hasNewVersion; }
            set { SetProperty(ref hasNewVersion, value); }
        }

        private string newVersionDesc;
        public string NewVersionDesc
        {
            get { return newVersionDesc; }
            set { SetProperty(ref newVersionDesc, value); }
        }

        private ICommand updateNowCmd;
        public ICommand UpdateNowCmd
        {
            get { return updateNowCmd; }
            set { SetProperty(ref updateNowCmd, value); }
        }

        private ICommand closePageCmd;
        public ICommand ClosePageCmd
        {
            get { return closePageCmd; }
            set { SetProperty(ref closePageCmd, value); }
        }
    }
}
