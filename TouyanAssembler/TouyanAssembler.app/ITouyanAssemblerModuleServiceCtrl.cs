using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouyanAssembler.app
{
    /// <summary>
    /// 投研 TouyanAssembler 模块中的服务管理的基协议。模块中的所有服务管理实现类都要实现该协议。
    /// </summary>
    internal interface ITouyanAssemblerModuleServiceCtrl
    {
        void Initialize();

        void Shutdown();
    }
}
