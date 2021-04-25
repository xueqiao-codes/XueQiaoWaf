using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    public class TabWorkspace
    {
        /// <summary>
        /// WorkspaceKey
        /// </summary>
        public string WorkspaceKey { get; set; }

        /// <summary>
        /// 工作空间类型。参考 XueQiaoConstans 的 `工作空间类型`
        /// </summary>
        public int WorkspaceType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string WorkspaceDesc { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 子账户 id
        /// </summary>
        public long SubAccountId { get; set; }
    }
}
