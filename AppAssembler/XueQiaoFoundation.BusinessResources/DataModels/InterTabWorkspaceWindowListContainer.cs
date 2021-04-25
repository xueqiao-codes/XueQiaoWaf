using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 工作空间拆分窗口列表容器
    /// </summary>
    public class InterTabWorkspaceWindowListContainer
    {
        public InterTabWorkspaceWindowListContainer()
        {
            Windows = new List<InterTabWorkspaceWindowContainer>();
        }

        public List<InterTabWorkspaceWindowContainer> Windows { get; private set; }
    }
}
