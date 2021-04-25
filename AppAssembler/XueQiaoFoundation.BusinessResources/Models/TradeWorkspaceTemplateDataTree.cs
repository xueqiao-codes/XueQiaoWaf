using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 交易工作空间模板数据树
    /// </summary>
    public class TradeWorkspaceTemplateDataTree
    {
        /// <summary>
        /// 模板列表
        /// </summary>
        public TradeTabWorkspaceTemplate[] Templates { get; set; }
    }

    /// <summary>
    /// 交易工作空间模板
    /// </summary>
    public class TradeTabWorkspaceTemplate
    {
        /// <summary>
        /// 模板 id
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string TemplateDesc { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public int CreateTimestamp { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public int LastUpdateTimestamp { get; set; }

        /// <summary>
        /// 包含的交易组件列表
        /// </summary>
        public TradeComp[] ChildComponents { get; set; }

    }
}
