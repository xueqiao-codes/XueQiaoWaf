using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 用户的订阅数据树
    /// </summary>
    public class UserSubscribeDataTree
    {
        /// <summary>
        /// 订阅的合约列表
        /// </summary>
        public SubscribeContractItem[] UserContracts { get; set; }

        /// <summary>
        /// 用户订阅合约的分组关系
        /// </summary>
        public SubscribeContractGroupRelation[] UserContractGroupRelations { get; set; }

        /// <summary>
        /// 用户订阅组合的分组关系
        /// </summary>
        public SubscribeComposeGroupRelation[] UserComposeGroupRelations { get; set; }

        /// <summary>
        /// 用户订阅合约列表的自定义分组列表
        /// </summary>
        public SubscribeDataCustomGroup[] UserContractListCustomGroups { get; set; }

        /// <summary>
        /// 用户订阅组合列表的自定义分组列表
        /// </summary>
        public SubscribeDataCustomGroup[] UserComposeListCustomGroups { get; set; }
    }
    
    public class SubscribeContractItem
    {
        // 合约 id
        public int ContractId { get; set; }

        // 添加时间
        public long AddTimestamp { get; set; }
    }

    public class SubscribeContractGroupRelation
    {
        // 合约 id
        public int ContractId { get; set; }

        // 所在分组 key 数组
        public string[] GroupKeys { get; set; }
    }

    public class SubscribeComposeGroupRelation
    {
        // 组合 id
        public long ComposeId { get; set; }

        // 所在分组 key 数组
        public string[] GroupKeys { get; set; }
    }

    /// <summary>
    /// 订阅的数据分组
    /// </summary>
    public class SubscribeDataCustomGroup
    {
        public string GroupKey { get; set; }

        public string GroupName { get; set; }
    }
}
