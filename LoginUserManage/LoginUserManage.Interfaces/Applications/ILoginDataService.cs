using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using xueqiao.trade.hosting.proxy;
using xueqiao.trade.hosting.terminal.ao;

namespace XueQiaoWaf.LoginUserManage.Interfaces.Applications
{
    public interface ILoginDataService 
    {
        /// <summary>
        /// 登录用户的公司 code
        /// </summary>
        string CompanyCode { get; }

        /// <summary>
        /// 登录用户的公司组
        /// </summary>
        ProxyCompanyGroup CompanyGroup { get; }
        
        /// <summary>
        /// 登录结果
        /// </summary>
        ProxyLoginResp ProxyLoginResp { get; } 
        
        /// <summary>
        /// 由登录结果信息生成的 landing info
        /// </summary>
        LandingInfo LandingInfo { get; }

        /// <summary>
        /// 登录用户的应用更新信息
        /// </summary>
        AppVersion LoginUserAppUpdateInfo { get; }

        /// <summary>
        /// 当前小哈投研授权信息
        /// </summary>
        XiaohaChartLandingInfo XiaohaTouyanAuthLandingInfo { get; }

        /// <summary>
        /// 是否已关联到投研用户
        /// </summary>
        bool HasLink2TouyanUser { get; }
        
        /// <summary>
        /// 是否是个人用户登录
        /// </summary>
        bool IsPersonalUserLogin { get; }
    }
}
