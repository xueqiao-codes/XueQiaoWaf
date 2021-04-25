using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;

namespace Touyan.Interface.application
{
    /// <summary>
    /// 投研授权用户已登录 delegate
    /// </summary>
    public delegate void TouyanAuthUserHasLogined();
    
    /// <summary>
    /// 投研授权用户已登出 delegate
    /// </summary>
    /// <param name="lastLoginLandingInfo">上次登录信息</param>
    public delegate void TouyanAuthUserHasLogouted(XiaohaChartLandingInfo lastLoginLandingInfo);

    /// <summary>
    /// 投研授权用户关联至投研用户的关联状态变化 delegate
    /// <para name="authUserLoginLandingInfo">授权用户的登录信息</para>
    /// <para name="linkedState">关联状态</para>
    /// </summary>
    public delegate void TouyanAuthUserLink2TouyanUserStateChanged(XiaohaChartLandingInfo authUserLoginLandingInfo, bool linkedState);

    /// <summary>
    /// 投研授权用户登录信息服务协议
    /// </summary>
    public interface ITouyanAuthUserLoginService
    {
        /// <summary>
        /// 当前投研用户的登录信息
        /// </summary>
        XiaohaChartLandingInfo TouyanAuthUserLoginLandingInfo { get; }

        /// <summary>
        /// 投研用户已登录事件
        /// </summary>
        event TouyanAuthUserHasLogined TouyanAuthUserHasLogined;

        /// <summary>
        /// 投研用户已登出事件
        /// </summary>
        event TouyanAuthUserHasLogouted TouyanAuthUserHasLogouted;
        
        /// <summary>
        /// 是否存在“用户数据管理可以注册账户”的功能
        /// </summary>
        bool HasFeature_UserDataManageRegister { get; }

        /// <summary>
        /// 是否存在“用户数据管理可以关联至投研用户”的功能
        /// </summary>
        bool HasFeature_UserDataManageLink2TouyanUser { get; }

        /// <summary>
        /// 授权用户是否已关联了投研用户
        /// </summary>
        bool HasLinked2TouyanUserOfTouyanAuthUser { get; }

        /// <summary>
        /// 投研授权用户已关联投研用户的事件
        /// </summary>
        event TouyanAuthUserLink2TouyanUserStateChanged TouyanAuthUserLink2TouyanUserStateChanged;

        /// <summary>
        /// 显示投研授权用户登录弹窗
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        /// <returns>True for logined, false for failed, null for canceled.</returns>
        bool? ShowTouyanAuthUserLoginDialog(object dialogOwner);

        /// <summary>
        /// 显示投研授权用户注册弹窗
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        /// <returns>True for logined, false for failed, null for canceled.</returns>
        bool? ShowTouyanAuthUserRegisterDialog(object dialogOwner);

        /// <summary>
        /// 显示投研授权用户关联投研用户的弹窗
        /// </summary>
        /// <param name="dialogOwner">The dialog owner.</param>
        /// <returns>True for logined, false for failed, null for canceled.</returns>
        bool? ShowTouyanAuthUserLink2TouyanUserDialog(object dialogOwner);
    }
}
