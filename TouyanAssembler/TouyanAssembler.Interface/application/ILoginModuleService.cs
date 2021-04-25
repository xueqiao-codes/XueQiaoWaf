using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;

namespace TouyanAssembler.Interface.application
{
    /// <summary>
    /// 已登录 delegate
    /// </summary>
    public delegate void HasLogined();

    /// <summary>
    /// 正在登出 delegate
    /// </summary>
    /// <param name="currentLoginLandingInfo">当前登录信息</param>
    public delegate void IsLogouting(XiaohaChartLandingInfo currentLoginLandingInfo);

    /// <summary>
    /// 已登出 delegate
    /// </summary>
    /// <param name="lastLoginLandingInfo">上次登录信息</param>
    public delegate void HasLogouted(XiaohaChartLandingInfo lastLoginLandingInfo);


    /// <summary>
    /// 登录模块服务
    /// </summary>
    public interface ILoginModuleService
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
        /// Show a dialog which allows the user to login .
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        /// <param name="dialogContentRendered">dialog content rendered delegate.</param>
        /// <returns>True for logined, false for failed, null for canceled.</returns>
        bool? ShowLoginDialog(object dialogOwner, Action dialogContentRendered);

        /// <summary>
        /// Show a dialog which allows the user to register .
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        /// <param name="dialogContentRendered">dialog content rendered delegate.</param>
        /// <returns>True for registered, false for failed, null for canceled.</returns>
        bool? ShowRegisterDialog(object dialogOwner, Action dialogContentRendered);

        /// <summary>
        /// 处理登出
        /// </summary>
        void DoSignout();
    }
}
