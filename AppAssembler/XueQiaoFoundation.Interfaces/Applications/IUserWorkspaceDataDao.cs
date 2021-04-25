using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.trade.hosting.terminal.ao;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoFoundation.Interfaces.Applications
{
    public interface IUserWorkspaceDataDao
    {
        /// <summary>
        /// 获取工作空间数据
        /// </summary>
        /// <param name="landingInfo">登录信息</param>
        /// <param name="dataVersion">获取的数据版本</param>
        /// <returns>工作空间数据</returns>
        WorkspaceWindowTree GetUserWorkspaceData(LandingInfo landingInfo, out int? dataVersion);

        /// <summary>
        /// 保存工作空间数据
        /// </summary>
        /// <param name="landingInfo">登录信息</param>
        /// <param name="userWorkspaceDataTree">数据</param>
        /// <param name="dataVersion">要保存的数据版本</param>
        /// <returns>如果成功。返回保存的数据版本号</returns>
        int? SaveUserWorkspaceData(LandingInfo landingInfo, WorkspaceWindowTree userWorkspaceDataTree, int dataVersion);

    }
}
