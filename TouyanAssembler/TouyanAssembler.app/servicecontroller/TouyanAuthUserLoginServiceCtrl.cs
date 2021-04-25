using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.application;
using TouyanAssembler.Interface.application;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;

namespace TouyanAssembler.app.servicecontroller
{
    [   
        Export(typeof(ITouyanAuthUserLoginService)),
        Export(typeof(ITouyanAssemblerModuleServiceCtrl)), 
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class TouyanAuthUserLoginServiceCtrl : ITouyanAuthUserLoginService, ITouyanAssemblerModuleServiceCtrl
    {
        private readonly ILoginDataService loginDataService;
        private readonly ILoginModuleService loginModuleService;

        [ImportingConstructor]
        public TouyanAuthUserLoginServiceCtrl(
            ILoginDataService loginDataService,
            ILoginModuleService loginModuleService)
        {
            this.loginDataService = loginDataService;
            this.loginModuleService = loginModuleService;
        }

        public void Initialize()
        {
            loginModuleService.HasLogined += LoginModuleService_HasLogined;
            loginModuleService.HasLogouted += LoginModuleService_HasLogouted;
        }
        
        public void Shutdown()
        {
            loginModuleService.HasLogined -= LoginModuleService_HasLogined;
            loginModuleService.HasLogouted -= LoginModuleService_HasLogouted;
        }

        #region ITouyanAuthUserLoginService

        public XiaohaChartLandingInfo TouyanAuthUserLoginLandingInfo => loginDataService.LoginLandingInfo;

        public event TouyanAuthUserHasLogined TouyanAuthUserHasLogined;

        public event TouyanAuthUserHasLogouted TouyanAuthUserHasLogouted;

        public bool HasFeature_UserDataManageRegister => true;

        public bool HasFeature_UserDataManageLink2TouyanUser => false;

        public bool HasLinked2TouyanUserOfTouyanAuthUser => false;

        public event TouyanAuthUserLink2TouyanUserStateChanged TouyanAuthUserLink2TouyanUserStateChanged;

        public bool? ShowTouyanAuthUserLoginDialog(object dialogOwner)
        {
            return loginModuleService.ShowLoginDialog(dialogOwner, null);
        }

        public bool? ShowTouyanAuthUserRegisterDialog(object dialogOwner)
        {
            return loginModuleService.ShowRegisterDialog(dialogOwner, null);
        }

        public bool? ShowTouyanAuthUserLink2TouyanUserDialog(object dialogOwner)
        {
            return false;
        }

        #endregion

        private void LoginModuleService_HasLogined()
        {
            TouyanAuthUserHasLogined?.Invoke();
        }

        private void LoginModuleService_HasLogouted(XiaohaChartLandingInfo lastLoginLandingInfo)
        {
            TouyanAuthUserHasLogouted?.Invoke(lastLoginLandingInfo);
        }

    }
}
