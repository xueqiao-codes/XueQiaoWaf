using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Applications.DataModels;
using System.ComponentModel.Composition;
using Manage.Applications.Services;
using Prism.Events;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Interfaces.Helper;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Models;

namespace Manage.Applications.ServiceControllers
{
    [Export(typeof(IPositionVerifyTradeInputItemsCtrl)), Export(typeof(IManageModuleSingletonController)), 
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class PositionVerifyTradeInputItemsCtrl : IPositionVerifyTradeInputItemsCtrl, IManageModuleSingletonController
    {
        private readonly PositionVerifyTradeInputItemsService xqPreviewTradeItemsService;
        private readonly IEventAggregator eventAggregator;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly IContractItemTreeQueryController contractItemTreeQueryController;

        private readonly object xqPreviewTradeItemsLock = new object();
        private readonly Dictionary<string, PositionVerifyTradeInputDM> xqPreviewTradeItemsDict
            = new Dictionary<string, PositionVerifyTradeInputDM>();

        [ImportingConstructor]
        public PositionVerifyTradeInputItemsCtrl(PositionVerifyTradeInputItemsService xqPreviewTradeItemsService,
            IEventAggregator eventAggregator,
            Lazy<ILoginUserManageService> loginUserManageService,
            IContractItemTreeQueryController contractItemTreeQueryController)
        {
            this.xqPreviewTradeItemsService = xqPreviewTradeItemsService;
            this.eventAggregator = eventAggregator;
            this.loginUserManageService = loginUserManageService;
            this.contractItemTreeQueryController = contractItemTreeQueryController;

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public void Shutdown()
        {
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            RemoveAllItems();
        }

        public PositionVerifyTradeInputDM AddOrUpdateItem(string previewItemKey, long fundAccountId, long? belongVerifyDailySec,
        Func<bool, XqPreviewInputTradeItemUpdateTemplate> updateTemplateFactory)
        {
            PositionVerifyTradeInputDM optModel = null;
            lock (xqPreviewTradeItemsLock)
            {
                PositionVerifyTradeInputDM existModel = null;
                if (!xqPreviewTradeItemsDict.TryGetValue(previewItemKey, out existModel))
                {
                    optModel = new PositionVerifyTradeInputDM(previewItemKey, fundAccountId, belongVerifyDailySec);
                    xqPreviewTradeItemsDict[previewItemKey] = optModel;
                    DispatcherHelper.CheckBeginInvokeOnUI(() => 
                    {
                        xqPreviewTradeItemsService.XqPreviewTradeItems.Add(optModel);
                    });
                }
                else
                {
                    optModel = existModel;
                }
                var updateTemplate = updateTemplateFactory?.Invoke(existModel != null);
                if (updateTemplate != null)
                {
                    UpdateItemModelWithTemplate(optModel, updateTemplate);
                }
            }
            return optModel;
        }
        
        public void UpdateItem(string previewItemKey, Func<XqPreviewInputTradeItemUpdateTemplate> updateTemplateFactory)
        {
            lock (xqPreviewTradeItemsLock)
            {
                PositionVerifyTradeInputDM existModel = null;
                if (xqPreviewTradeItemsDict.TryGetValue(previewItemKey, out existModel))
                {
                    var updateTemplate = updateTemplateFactory?.Invoke();
                    if (updateTemplate != null)
                    {
                        UpdateItemModelWithTemplate(existModel, updateTemplate);
                    }
                }
            }
        }
        
        public void RemoveItem(string previewItemKey)
        {
            lock (xqPreviewTradeItemsLock)
            {
                PositionVerifyTradeInputDM existModel = null;
                if (xqPreviewTradeItemsDict.TryGetValue(previewItemKey, out existModel))
                {
                    xqPreviewTradeItemsDict.Remove(previewItemKey);
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        xqPreviewTradeItemsService.XqPreviewTradeItems.Remove(existModel);
                    });
                }
            }
        }
        
        public IEnumerable<PositionVerifyTradeInputDM> AllItems
        {
            get
            {
                IEnumerable<PositionVerifyTradeInputDM> items = null;
                lock (xqPreviewTradeItemsLock)
                {
                    items = xqPreviewTradeItemsDict.Values.ToArray();
                }
                return items;
            }
        }

        public int AllItemsCount
        {
            get
            {
                lock (xqPreviewTradeItemsLock)
                {
                    return xqPreviewTradeItemsDict.Count;
                }
            }
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            RemoveAllItems();
        }

        private void UpdateItemModelWithTemplate(PositionVerifyTradeInputDM itemModel, XqPreviewInputTradeItemUpdateTemplate updateTemplate)
        {
            if (itemModel == null || updateTemplate == null) return;

            if (updateTemplate.ContractId != null)
            {
                itemModel.ContractId = updateTemplate.ContractId;
                var contractContainer = new TargetContract_TargetContractDetail(updateTemplate.ContractId.Value);
                XueQiaoFoundationHelper.SetupTargetContract_ContractDetail(contractContainer,
                    contractItemTreeQueryController, XqContractNameFormatType.CommodityAcronym_Code_ContractCode);
                itemModel.ContractDetailContainer = contractContainer;
            }            

            if (updateTemplate.TradeTimestamp != null)
                itemModel.TradeTimestamp = updateTemplate.TradeTimestamp.Value;

            if (updateTemplate.Direction != null)
                itemModel.Direction = updateTemplate.Direction.Value;

            if (updateTemplate.Price != null)
                itemModel.Price = updateTemplate.Price.Value;

            if (updateTemplate.Quantity != null)
                itemModel.Quantity = updateTemplate.Quantity.Value;
        }

        private void RemoveAllItems()
        {
            lock (xqPreviewTradeItemsLock)
            {
                xqPreviewTradeItemsDict.Clear();
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    xqPreviewTradeItemsService.XqPreviewTradeItems.Clear();
                });
            }
        }
    }
}
