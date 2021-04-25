using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using TouyanAssembler.app.view;

namespace TouyanAssembler.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class AppInfoVM : ViewModel<AppInfoView>
    {
        [ImportingConstructor]
        public AppInfoVM(AppInfoView view) : base(view)
        {
            ApiEnvironments = (lib.xqclient_base.thriftapi_mediation.Environment[])Enum.GetValues(typeof(lib.xqclient_base.thriftapi_mediation.Environment));
        }

        private lib.xqclient_base.thriftapi_mediation.Environment selectedApiEnvironment;
        public lib.xqclient_base.thriftapi_mediation.Environment SelectedApiEnvironment
        {
            get { return selectedApiEnvironment; }
            set { SetProperty(ref selectedApiEnvironment, value); }
        }

        public lib.xqclient_base.thriftapi_mediation.Environment[] ApiEnvironments { get; private set; }
        
        private bool showApiEnvironmentSelectBox;
        public bool ShowApiEnvironmentSelectBox
        {
            get { return showApiEnvironmentSelectBox; }
            set { SetProperty(ref showApiEnvironmentSelectBox, value); }
        }
        
        private string currentVersionStr;
        public string CurrentVersionStr
        {
            get { return currentVersionStr; }
            set { SetProperty(ref currentVersionStr, value); }
        }

        private ICommand checkNewVersionCmd;
        public ICommand CheckNewVersionCmd
        {
            get { return checkNewVersionCmd; }
            set { SetProperty(ref checkNewVersionCmd, value); }
        }
        
        private int newVersionDownloadProgress;
        public int NewVersionDownloadProgress
        {
            get { return newVersionDownloadProgress; }
            set { SetProperty(ref newVersionDownloadProgress, value); }
        }

        private bool isNewVersionDownloading;
        public bool IsNewVersionDownloading
        {
            get { return isNewVersionDownloading; }
            set { SetProperty(ref isNewVersionDownloading, value); }
        }

    }
}
