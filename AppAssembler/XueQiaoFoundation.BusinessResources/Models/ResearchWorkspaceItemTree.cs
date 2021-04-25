using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 投研工作空间的数据树
    /// </summary>
    public class ResearchWorkspaceItemTree
    {
        /// <summary>
        /// 工作空间基本信息
        /// </summary>
        public TabWorkspace Workspace { get; set; }

        /// <summary>
        /// 包含的投研组件
        /// </summary>
        public ResearchComp[] ResearchComponents { get; set; }
    }
}
