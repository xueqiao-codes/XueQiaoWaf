using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 持仓预分配(Position Preview Assign)数据树
    /// </summary>
    public class PPADataTree
    {
        /// <summary>
        /// 持仓预分配项列表
        /// </summary>
        public PPAItem[] PAItems { get; set; }
    }

    /// <summary>
    /// 持仓预分配(Position Preview Assign)项
    /// </summary>
    public class PPAItem
    {
        // 未分配成交项的 key
        public string UATKey { get; set; }

        /// <summary>
        /// 所属资账户 id
        /// </summary>
        public long FAccId { get; set; }

        /// <summary>
        /// 所属合约 id
        /// </summary>
        public int CId { get; set; }

        /// <summary>
        /// 分配的子账户 id
        /// </summary>
        public long SubAccId { get; set; }

        /// <summary>
        /// 分配数量
        /// </summary>
        public int Vol { get; set; }
    }
}
