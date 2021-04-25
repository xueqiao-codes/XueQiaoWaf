using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 交易工作空间的数据树
    /// </summary>
    public class TradeWorkspaceItemTree
    {
        /// <summary>
        /// 工作空间基本信息
        /// </summary>
        public TabWorkspace Workspace { get; set; }

        /// <summary>
        /// 包含的交易组件
        /// </summary>
        public TradeComp[] TradeComponents { get; set; }
    }
}
