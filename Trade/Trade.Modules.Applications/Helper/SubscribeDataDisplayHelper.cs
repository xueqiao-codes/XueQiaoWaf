using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    public static class SubscribeDataDisplayHelper
    {
        /// <summary>
        /// 为<see cref="SubscribeDataGroupsDataRoot"/>配置固定的分组项
        /// </summary>
        /// <param name="dataRoot"></param>
        public static void ConfigFixedGroups(this SubscribeDataGroupsDataRoot dataRoot)
        {
            if (dataRoot == null) return;

            // 设置合约订阅列表的固定分组
            var contractGroups = new List<SubscribeDataGroup>(dataRoot.ContractGroups ?? new SubscribeDataGroup[]{});
            
            contractGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.ComposeRelated);
            // 不显示“组合相关”分组
            //contractGroups.Insert(0, SubscribeDataGroup.CreateGroup_ComposeRelated());

            contractGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.IsExpired);
            contractGroups.Insert(0, SubscribeDataGroup.CreateGroup_IsExpired());

            contractGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.OnTrading);
            contractGroups.Insert(0, SubscribeDataGroup.CreateGroup_OnTrading());

            contractGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.ExistPosition);
            contractGroups.Insert(0, SubscribeDataGroup.CreateGroup_ExistPosition());

            contractGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.All);
            contractGroups.Insert(0, SubscribeDataGroup.CreateGroup_All());

            dataRoot.ContractGroups = contractGroups.ToArray();

            // 设置组合订阅列表的固定分组
            var composeGroups = new List<SubscribeDataGroup>(dataRoot.ComposeGroups ?? new SubscribeDataGroup[] { });

            composeGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.IsExpired);
            composeGroups.Insert(0, SubscribeDataGroup.CreateGroup_IsExpired());

            composeGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.OnTrading);
            composeGroups.Insert(0, SubscribeDataGroup.CreateGroup_OnTrading());

            composeGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.ExistPosition);
            composeGroups.Insert(0, SubscribeDataGroup.CreateGroup_ExistPosition());

            composeGroups.RemoveAll(i => i.GroupType == SubscribeDataGroupType.All);
            composeGroups.Insert(0, SubscribeDataGroup.CreateGroup_All());
            
            dataRoot.ComposeGroups = composeGroups.ToArray();
        }
    }
}
