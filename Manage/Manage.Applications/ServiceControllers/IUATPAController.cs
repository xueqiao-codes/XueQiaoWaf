using Manage.Applications.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Applications.ServiceControllers
{
    /// <summary>
    /// 未分配成交项预分配管理 controller
    /// </summary>
    internal interface IUATPAController
    {
        /// <summary>
        /// 刷新某个资金账户的未分配持仓列表。如果 fundAccountId 未指定，则刷新所有资金账户的未分配持仓列表。
        /// <paramref name="fundAccountId">指定资金账户 id</paramref>
        /// </summary>
        void RefreshUATItemsIfNeed(long? fundAccountId);
        
        /// <summary>
        /// 强制刷新某个资金账户的未分配持仓列表。如果 fundAccountId 未指定，则强制刷新所有资金账户的未分配持仓列表。
        /// <paramref name="fundAccountId">指定资金账户 id</paramref>
        /// </summary>
        void RefreshUATItemsForce(long? fundAccountId);
        
        /// <summary>
        /// 资金账号的未分配持仓列表刷新状态改变事件
        /// </summary>
        event FundAccountRelatedDataRefreshStateChanged UATItemsRefreshStateChanged;

        /// <summary>
        /// 添加或修改持仓预分配项
        /// </summary>
        /// <param name="PAItemKey">预分配项 key 对象</param>
        /// <param name="updateTemplateFactory">预分配项修改模板获取方法。arg0:存在的该预分配项，arg1:返回的修改模板</param>
        /// <returns>预分配 data model。如果添加不成功返回 null</returns>
        PositionPreviewAssignDM AddOrUpdatePAItem(PositionPreviewAssignItemKey PAItemKey, Func<PositionPreviewAssignDM, PAItemUpdateTemplate> updateTemplateFactory);

        /// <summary>
        /// 删除持仓预分配项
        /// </summary>
        /// <param name="rmPAItemKeys">要删除的预分配项 key 对象列表</param>
        void RemovePAItemsWithKey(IEnumerable<PositionPreviewAssignItemKey> rmPAItemKeys);
            
    }

    /// <summary>
    /// 持仓预分配项修改模板
    /// </summary>
    public class PAItemUpdateTemplate
    {
        public int? Volume { get; set; }
    }
}
