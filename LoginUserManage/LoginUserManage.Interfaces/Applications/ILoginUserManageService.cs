using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using xueqiao.trade.hosting.proxy;

namespace XueQiaoWaf.LoginUserManage.Interfaces.Applications
{
    /// <summary>
    /// 已登录 delegate
    /// </summary>
    public delegate void HasLogined();

    /// <summary>
    /// 正在登出 delegate
    /// </summary>
    /// <param name="currentLoginResp">当前登录信息</param>
    public delegate void IsLogouting(ProxyLoginResp currentLoginResp);

    /// <summary>
    /// 已登出 delegate
    /// </summary>
    /// <param name="lastLoginResp">上次登录信息</param>
    public delegate void HasLogouted(ProxyLoginResp lastLoginResp);

    /// <summary>
    /// 授权的投研用户已登出 delegate
    /// </summary>
    /// <param name="lastAuthUserLandingInfo">上次授权的登录信息</param>
    public delegate void AuthTouyanUserHasLogouted(XiaohaChartLandingInfo lastAuthUserLandingInfo);

    /// <summary>
    ///应用更新信息已查询 delegate
    /// </summary>
    /// <param name="appVersion"></param>
    public delegate void AppUpdateInfoQueried(AppVersion appVersion);

    /// <summary>
    /// 关联至投研用户的关联状态变化 delegate
    /// <para name="linkedState">关联状态</para>
    /// </summary>
    public delegate void Link2TouyanUserStateChanged(bool linkedState);

    /// <summary>
    /// Exposes the login user manage functionality to other modules.
    /// </summary>
    public interface ILoginUserManageService
    {
        /// <summary>
        /// 已登录事件
        /// </summary>
        event HasLogined HasLogined;

        /// <summary>
        /// 正在登出事件
        /// </summary>
        event IsLogouting IsLogouting;

        /// <summary>
        /// 已登出事件
        /// </summary>
        event HasLogouted HasLogouted;

        /// <summary>
        /// 授权的投研用户已登出事件
        /// </summary>
        event AuthTouyanUserHasLogouted AuthTouyanUserHasLogouted;

        /// <summary>
        /// 登录用户的应用更新信息已查询事件
        /// </summary>
        event AppUpdateInfoQueried LoginUserAppUpdateInfoQueried;

        /// <summary>
        /// Show a dialog which allows the user to login.
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        /// <param name="dialogContentRendered">Login dialog content rendered delegate.</param>
        /// <returns>True for logined, false for failed, null for canceled.</returns>
        bool? ShowLoginDialog(object dialogOwner, Action dialogContentRendered);
        
        /// <summary>
        /// 处理登出
        /// </summary>
        void DoSignout();

        /// <summary>
        /// Show a dialog to update login password.
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        void ShowLoginPwdUpdateDialog(object dialogOwner);

        /// <summary>
        /// 显示登录用户的应用版本信息弹窗
        /// </summary>
        /// <param name="dialogOwner"></param>
        void ShowUserAppVersionInfoDialog(object dialogOwner);

        /// <summary>
        /// 查询登录用户的应用更新信息
        /// </summary>
        /// <returns></returns>
        Task<IInterfaceInteractResponse<AppVersion>> QueryLoginUserAppUpdateInfo();

        /// <summary>
        /// 相同的用户是否正在本台设备登录。
        /// </summary>
        /// <param name="apilib_environment">api 库的环境</param>
        /// <param name="companyCode">用户的所属公司 code</param>
        /// <param name="companyGroupCode">用户的所属公司组 code</param>
        /// <param name="userName">用户登录名</param>
        /// <param name="isPersonalUser">是否为个人用户</param>
        /// <returns></returns>
        bool IsSameUserLoginAtThisDevice(lib.xqclient_base.thriftapi_mediation.Environment apilib_environment,
            string companyCode, string companyGroupCode, string userName, bool isPersonalUser);


        /// <summary>
        /// 显示关联到投研用户的关联弹窗
        /// </summary>
        /// <param name="dialogOwner"></param>
        /// <returns></returns>
        bool? ShowLink2TouyanUserDialog(object dialogOwner);

        /// <summary>
        /// 关联到投研用户的关联状态变化事件
        /// </summary>
        event Link2TouyanUserStateChanged Link2TouyanUserStateChanged;
    }
}
