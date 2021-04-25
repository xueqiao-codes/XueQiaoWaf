using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications
{
    /// <summary>
    /// 登录模块中的服务管理的基协议。模块中的所有服务管理实现类都要实现该协议。
    /// </summary>
    internal interface ILoginModuleServiceCtrl
    {
        void Initialize();

        void Shutdown();
    }
}
