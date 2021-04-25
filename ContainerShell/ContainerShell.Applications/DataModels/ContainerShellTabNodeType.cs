using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShell.Applications.DataModels
{
    /// <summary>
    /// tab 类型
    /// </summary>
    public enum ContainerShellTabNodeType
    {
        Trade = 1,      // 交易
        Manage = 10,    // 管理
        Research = 20,  // 投研
        PersonalUserTradeManage = 30, // 个人用户交易管理 
    }
}
