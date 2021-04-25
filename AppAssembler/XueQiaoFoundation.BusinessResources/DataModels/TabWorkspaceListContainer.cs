using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 工作空间列表容器
    /// </summary>
    public class TabWorkspaceListContainer
    {
        public TabWorkspaceListContainer()
        {
            Workspaces = new List<TabWorkspace>();
        }

        public List<TabWorkspace> Workspaces { get; private set; }
    }
}
