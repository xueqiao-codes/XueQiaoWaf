using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoWaf.Trade.Interfaces.DataModels
{
    /// <summary>
    /// 订阅数据的分组 data root
    /// </summary>
    public class SubscribeDataGroupsDataRoot
    {
        public SubscribeDataGroupsDataRoot()
        {
        }
        
        /// <summary>
        /// 合约分组列表
        /// </summary>
        public IEnumerable<SubscribeDataGroup> ContractGroups { get; set; }

        /// <summary>
        /// 组合分组列表
        /// </summary>
        public IEnumerable<SubscribeDataGroup> ComposeGroups { get; set; }
    }
    
    /// <summary>
    /// 分组
    /// </summary>
    public class SubscribeDataGroup : Model
    {
        public const string Group_All_SharedKey = "a2dd5cf6-013a-4b68-8557-431f13d4cf4c";
        public const string Group_ExistPosition_SharedKey = "72e1245d-400e-462a-80c3-adf1550feeeb";
        public const string Group_OnTrading_SharedKey = "723b8bd0-4c91-4b27-a6b2-1682ce0f2c41";
        public const string Group_ComposeRelated_SharedKey = "f08dc7b0-c55a-49af-956c-91b2761802cf";
        public const string Group_IsExpired_SharedKey = "CFA04952-37B3-4403-AF96-C845CEDCEC9E";

        public SubscribeDataGroup(SubscribeDataGroupType groupType, string groupKey)
        {
            if (string.IsNullOrEmpty(groupKey))
            {
                throw new ArgumentException("groupKey");
            }

            this.GroupType = groupType;
            this.GroupKey = groupKey;
        }

        public SubscribeDataGroupType GroupType { get; private set; }

        public string GroupKey { get; private set; }

        private string groupName;
        public string GroupName
        {
            get { return groupName; }
            set { SetProperty(ref groupName, value); }
        }

        public static SubscribeDataGroup CreateGroup_All()
        {
            return new SubscribeDataGroup(SubscribeDataGroupType.All, Group_All_SharedKey);
        }

        public static SubscribeDataGroup CreateGroup_ExistPosition()
        {
            return new SubscribeDataGroup(SubscribeDataGroupType.ExistPosition, Group_ExistPosition_SharedKey);
        }

        public static SubscribeDataGroup CreateGroup_OnTrading()
        {
            return new SubscribeDataGroup(SubscribeDataGroupType.OnTrading, Group_OnTrading_SharedKey);
        }

        public static SubscribeDataGroup CreateGroup_ComposeRelated()
        {
            return new SubscribeDataGroup(SubscribeDataGroupType.ComposeRelated, Group_ComposeRelated_SharedKey);
        }

        public static SubscribeDataGroup CreateGroup_IsExpired()
        {
            return new SubscribeDataGroup(SubscribeDataGroupType.IsExpired, Group_IsExpired_SharedKey);
        }
    }

    /// <summary>
    /// 分组类型
    /// </summary>
    public enum SubscribeDataGroupType
    {
        All             = 1,   
        ExistPosition   = 2,
        OnTrading       = 3,
        ComposeRelated  = 4,
        Custom          = 5,
        IsExpired       = 6, // 已过期
    }
}
