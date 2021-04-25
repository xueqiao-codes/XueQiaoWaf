using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace ContainerShell.Interfaces.Applications
{
    /// <summary>
    /// 工作空间视图控制器协议
    /// </summary>
    public interface IWorkspaceItemViewCtrl
    {
        /// <summary>
        /// 工作空间信息
        /// </summary>
        TabWorkspace Workspace { get; }

        /// <summary>
        /// 内容视图
        /// </summary>
        object ContentView { get; }

        void Initialize();

        void Run();

        void Shutdown();
    }
}
