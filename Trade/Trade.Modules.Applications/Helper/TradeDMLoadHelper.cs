using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.trade.hosting;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    public static class TradeDMLoadHelper
    {
        /// <summary>
        /// 创建组合标的腿成交概要列表
        /// </summary>
        /// <param name="legContractIds"></param>
        /// <param name="belongComposeId"></param>
        /// <param name="legTradeSummaryPriceTypeFactory"></param>
        /// <param name="composeGraphCacheController"></param>
        /// <param name="composeGraphQueryController"></param>
        /// <param name="userComposeViewCacheController"></param>
        /// <param name="summarisLegInfoDidConfigured">概要的腿信息已配置回调</param>
        /// <returns></returns>
        public static IEnumerable<TargetComposeLegTradeSummary> CreateTargetComposeLegTradeSummarys(IEnumerable<int> legContractIds, 
            long belongComposeId,
            Func<int, LegTradeSummaryPriceType> legTradeSummaryPriceTypeFactory,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController,
            Action<IEnumerable<TargetComposeLegTradeSummary>> summarisLegInfoDidConfigured)
        {
            if (legContractIds == null) return null;

            if (legTradeSummaryPriceTypeFactory == null) throw new ArgumentNullException("legTradeSummaryPriceTypeFactory");
            if (composeGraphCacheController == null) throw new ArgumentNullException("composeCacheController");
            if (composeGraphQueryController == null) throw new ArgumentNullException("composeQueryController");
            if (userComposeViewCacheController == null) throw new ArgumentNullException("userComposeViewCacheController");

            var legTradeSummarys = legContractIds.Select(_legId => new TargetComposeLegTradeSummary(belongComposeId, _legId, legTradeSummaryPriceTypeFactory.Invoke(_legId)))
                .ToArray();
            
            var configureLegsWithCompose = new Action<NativeComposeGraph>(__compose =>
            {
                if (legTradeSummarys?.Any() != true) return;
                if (__compose?.Legs == null) return;
                foreach (var leg in __compose.Legs)
                {
                    var tarLegSummary = legTradeSummarys.FirstOrDefault(i => i.LegContractId == leg.SledContractId);
                    if (tarLegSummary != null)
                    {
                        tarLegSummary.LegMeta = leg;
                    }
                }
                summarisLegInfoDidConfigured?.Invoke(legTradeSummarys);
            });

            var cachedCompose = composeGraphCacheController.GetCache(belongComposeId);
            if (cachedCompose == null)
            {
                cachedCompose = userComposeViewCacheController.AllCaches().Values.FirstOrDefault(i => i.ComposeGraph?.ComposeGraphId == belongComposeId)?.ComposeGraph;
            }

            if (cachedCompose != null)
            {
                configureLegsWithCompose(cachedCompose);
            }
            else
            {
                var queryHandler = new Action<IInterfaceInteractResponse<NativeComposeGraph>>(resp =>
                {
                    if (resp?.CorrectResult is NativeComposeGraph queriedCompose)
                    {
                        configureLegsWithCompose(queriedCompose);
                    }
                });
                var delegateReference = new ActionDelegateReference<IInterfaceInteractResponse<NativeComposeGraph>>(queryHandler, true);
                composeGraphQueryController.QueryComposeGraph(belongComposeId, delegateReference);
            }

            return legTradeSummarys;
        }

        /// <summary>
        /// 设置子账户的数据实体的数据
        /// </summary>
        /// <param name="subAccountFieldsContainer">子账户数据容器</param>
        public static void SetupSubAccountFields(SubAccountFieldsForTradeData subAccountFieldsContainer,
            IUserSubAccountRelatedItemQueryController subAccountRelatedItemQueryController,
            IUserSubAccountRelatedItemCacheController subAccountRelatedItemCacheController,
            IHostingUserQueryController hostingUserQueryController,
            IHostingUserCacheController hostingUserCacheController)
        {
            var subAccountId = subAccountFieldsContainer.SubAccountId;
            var subUserId = subAccountFieldsContainer.SubUserId;

            // 查询操作账户信息
            var cacheSubAccountRelated = subAccountRelatedItemCacheController.AllCaches()?.Values.FirstOrDefault(i => i.SubAccountId == subAccountId);
            if (cacheSubAccountRelated != null)
            {
                subAccountFieldsContainer.SubAccountName = cacheSubAccountRelated.SubAccountName;
            }
            else
            {
                var queryHandler = new Action<IInterfaceInteractResponse<IEnumerable<HostingSubAccountRelatedItem>>>(resp =>
                {
                    var queriedRelated = resp?.CorrectResult?.FirstOrDefault(i => i.SubAccountId == subAccountId);
                    if (queriedRelated == null) return;
                    subAccountFieldsContainer.SubAccountName = queriedRelated.SubAccountName;
                });
                var handlerReference = new ActionDelegateReference<IInterfaceInteractResponse<IEnumerable<HostingSubAccountRelatedItem>>>(queryHandler,
                    true);
                subAccountRelatedItemQueryController.QueryUserSubAccountRelatedItems(handlerReference);
            }

            if (subUserId.HasValue)
            {
                // 查询用户信息
                var cacheHostingUser = hostingUserCacheController.GetCache(subUserId.Value);
                if (cacheHostingUser != null)
                {
                    subAccountFieldsContainer.SubUserName = cacheHostingUser.LoginName;
                }
                else
                {
                    var queryHandler = new Action<IInterfaceInteractResponse<HostingUser>>(resp =>
                    {
                        var queriedUser = resp?.CorrectResult;
                        if (queriedUser == null) return;
                        subAccountFieldsContainer.SubUserName = queriedUser.LoginName;
                    });
                    var handlerReference = new ActionDelegateReference<IInterfaceInteractResponse<HostingUser>>(queryHandler,
                        true);
                    hostingUserQueryController.QueryUser(subUserId.Value, handlerReference);
                }
            }
        }
    }
}
