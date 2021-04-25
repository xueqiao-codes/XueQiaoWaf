using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.application;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.ServiceControllers
{
    [   
        Export(typeof(ITouyanAuthUserLoginService)),
        Export(typeof(ILoginModuleServiceCtrl)), 
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class TouyanAuthUserLoginServiceCtrl : ITouyanAuthUserLoginService, ILoginModuleServiceCtrl
    {
        private readonly ILoginDataService loginDataService;
        private readonly ILoginUserManageService loginUserManageService;

        [ImportingConstructor]
        public TouyanAuthUserLoginServiceCtrl(
            ILoginDataService loginDataService,
            ILoginUserManageService loginUserManageService)
        {
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
        }

        public void Initialize()
        {
            loginUserManageService.HasLogined += LoginModuleService_HasLogined;
            loginUserManageService.AuthTouyanUserHasLogouted += RecvAuthTouyanUserHasLogouted;
            loginUserManageService.Link2TouyanUserStateChanged += RecvLink2TouyanUserStateChanged;
        }
        
        public void Shutdown()
        {
            loginUserManageService.HasLogined -= LoginModuleService_HasLogined;
            loginUserManageService.AuthTouyanUserHasLogouted -= RecvAuthTouyanUserHasLogouted;
            loginUserManageService.Link2TouyanUserStateChanged -= RecvLink2TouyanUserStateChanged;
        }

        #region ITouyanAuthUserLoginService

        public XiaohaChartLandingInfo TouyanAuthUserLoginLandingInfo => loginDataService.XiaohaTouyanAuthLandingInfo;

        public event TouyanAuthUserHasLogined TouyanAuthUserHasLogined;

        public event TouyanAuthUserHasLogouted TouyanAuthUserHasLogouted;

        public bool HasFeature_UserDataManageRegister => false;

        public bool HasFeature_UserDataManageLink2TouyanUser => true;

        public bool HasLinked2TouyanUserOfTouyanAuthUser => loginDataService.HasLink2TouyanUser;

        public event TouyanAuthUserLink2TouyanUserStateChanged TouyanAuthUserLink2TouyanUserStateChanged;

        public bool? ShowTouyanAuthUserLoginDialog(object dialogOwner)
        {
            return loginUserManageService.ShowLoginDialog(dialogOwner, null);
        }

        public bool? ShowTouyanAuthUserRegisterDialog(object dialogOwner)
        {
            return false;
        }

        public bool? ShowTouyanAuthUserLink2TouyanUserDialog(object dialogOwner)
        {
            return loginUserManageService.ShowLink2TouyanUserDialog(dialogOwner);
        }

        #endregion

        private void LoginModuleService_HasLogined()
        {
            TouyanAuthUserHasLogined?.Invoke();
        }

        private void RecvAuthTouyanUserHasLogouted(XiaohaChartLandingInfo lastAuthUserLandingInfo)
        {
            TouyanAuthUserHasLogouted?.Invoke(lastAuthUserLandingInfo);
        }
        
        private void RecvLink2TouyanUserStateChanged(bool linkedState)
        {
            TouyanAuthUserLink2TouyanUserStateChanged?.Invoke(loginDataService.XiaohaTouyanAuthLandingInfo, linkedState);
        }
    }
}
