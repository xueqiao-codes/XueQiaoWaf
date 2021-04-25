using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.LoginUserManage.Modules.Applications.Views;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class LoginDialogContentViewModel : ViewModel<ILoginDialogContentView>
    {   
        [ImportingConstructor]
        public LoginDialogContentViewModel(ILoginDialogContentView view) : base(view)
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

        private object companyUserLoginOperateView;
        public object CompanyUserLoginOperateView
        {
            get { return companyUserLoginOperateView; }
            set { SetProperty(ref companyUserLoginOperateView, value); }
        }

        private object personalUserLoginOperateView;
        public object PersonalUserLoginOperateView
        {
            get { return personalUserLoginOperateView; }
            set { SetProperty(ref personalUserLoginOperateView, value); }
        }
    }
}
