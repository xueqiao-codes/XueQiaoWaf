﻿using Prism.Events;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using System.ComponentModel.Composition.Hosting;
using System;
using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;
using System.Threading.Tasks.Schedulers;
using ContainerShell.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.BusinessResources.Models;
using System.Timers;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using ContainerShell.Interfaces.DataModels;
using NativeModel.Trade;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.Constants;
using Newtonsoft.Json;
using IDLAutoGenerated.Util;
using xueqiao.trade.hosting.terminal.ao;
using xueqiao.trade.hosting;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using lib.xqclient_base.logger;
using XueQiaoWaf.Trade.Interfaces.DataModels;
using xueqiao.trade.hosting.proxy;
using lib.xqclient_base.thriftapi_mediation.Interface;
using AppAssembler.Interfaces.Applications;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// Responsible for the whole module.
    /// </summary>
    [Export(typeof(IModuleController)), Export(typeof(ITradeModuleService)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class ModuleController : IModuleController, ITradeModuleService
    {
        private readonly CompositionContainer compositionContainer;
        private readonly IEventAggregator eventAggregator;
        private readonly Lazy<ILoginDataService> loginDataService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly Lazy<IContainerShellService> containerShellService;
        private readonly Lazy<IAppAssemblerService> appAssemblerService;
        private readonly ExportFactory<TradeModuleRootViewController> moduleRootViewCtrlFactory;
        private readonly Lazy<ISubscribeContractController> subscribeContractCtrl;
        private readonly Lazy<ISubscribeComposeController> subscribeComposeCtrl;
        private readonly Lazy<IRelatedSubAccountItemsController> relatedSubAccountItemsCtrl;
        private readonly Lazy<IOrderItemsController> orderItemsCtrl;
        private readonly Lazy<ITradeItemsController> tradeItemsCtrl;
        private readonly Lazy<IPositionDiscreteItemsController> positionDiscreteItemsCtrl;
        private readonly Lazy<IXqTargetPositionItemsController> targetPositionItemsCtrl;
        private readonly Lazy<IFundItemsController> fundItemsCtrl;
        private readonly Lazy<IUserSettingSyncController> userSettingSyncCtrl;
        private readonly XQComposeOrderEPTService XQComposeOrderEPTService;

        private IEnumerable<ITradeModuleSingletonController> moduleSingletonCtrls;

        private TradeModuleRootViewController moduleRootViewCtrl;

        private readonly IDIncreaser orderIdIncreaser = new IDIncreaser();
        private readonly TaskFactory _orderOperateReqTaskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(2));

        // 同步云端用户设置数据定时器
        private Timer cloudUserSettingsSyncTimer;
        private readonly TaskFactory cloudUserSettingsSyncTimerTaskFactory = new TaskFactory(new OrderedTaskScheduler());

        private TradeWorkspaceDataRoot tradeWorkspaceDataRoot;
        private SubscribeDataGroupsDataRoot subscribeDataGroupsDataRoot;

        private readonly IDIncreaser tradeWorkspaceKeyIdIncreaser = new IDIncreaser();

        private bool shutdowned;
        
        [ImportingConstructor]
        public ModuleController(
            CompositionContainer compositionContainer,
            IEventAggregator eventAggregator,
            Lazy<ILoginDataService> loginDataService,
            Lazy<ILoginUserManageService> loginUserManageService,
            Lazy<IContainerShellService> containerShellService,
            Lazy<IAppAssemblerService> appAssemblerService,
            ExportFactory<TradeModuleRootViewController> moduleRootViewCtrlFactory,
            Lazy<ISubscribeContractController> subscribeContractCtrl,
            Lazy<ISubscribeComposeController> subscribeComposeCtrl,
            Lazy<IRelatedSubAccountItemsController> relatedSubAccountItemsCtrl,
            Lazy<IOrderItemsController> orderItemsCtrl,
            Lazy<ITradeItemsController> tradeItemsCtrl,
            Lazy<IPositionDiscreteItemsController> positionDiscreteItemsCtrl,
            Lazy<IXqTargetPositionItemsController> targetPositionItemsCtrl,
            Lazy<IFundItemsController> fundItemsCtrl,
            Lazy<IUserSettingSyncController> userSettingSyncCtrl,
            XQComposeOrderEPTService XQComposeOrderEPTService)
        {
            this.compositionContainer = compositionContainer;
            this.eventAggregator = eventAggregator;
            this.loginDataService = loginDataService;
            this.loginUserManageService = loginUserManageService;
            this.containerShellService = containerShellService;
            this.appAssemblerService = appAssemblerService;
            this.moduleRootViewCtrlFactory = moduleRootViewCtrlFactory;
            this.subscribeContractCtrl = subscribeContractCtrl;
            this.subscribeComposeCtrl = subscribeComposeCtrl;
            this.relatedSubAccountItemsCtrl = relatedSubAccountItemsCtrl;
            this.orderItemsCtrl = orderItemsCtrl;
            this.tradeItemsCtrl = tradeItemsCtrl;
            this.positionDiscreteItemsCtrl = positionDiscreteItemsCtrl;
            this.targetPositionItemsCtrl = targetPositionItemsCtrl;
            this.fundItemsCtrl = fundItemsCtrl;
            this.userSettingSyncCtrl = userSettingSyncCtrl;
            this.XQComposeOrderEPTService = XQComposeOrderEPTService;
        }
        
        public void Initialize()
        {
            loginUserManageService.Value.IsLogouting += RecvUserIsLogouting;
            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
            appAssemblerService.Value.AppShutdown += RecvAppShutdown;
            containerShellService.Value.XqInitializeDataInitialized += ReceiveInitialDataInitialized;

            moduleSingletonCtrls = compositionContainer.GetExportedValues<ITradeModuleSingletonController>()?.ToArray();
        }

        public void Run()
        {
        }

        public void Shutdown()
        {
            if (this.shutdowned) return;
            this.shutdowned = true;

            StopCloudUserSettingsSyncTimer();

            loginUserManageService.Value.IsLogouting -= RecvUserIsLogouting;
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            appAssemblerService.Value.AppShutdown -= RecvAppShutdown;
            containerShellService.Value.XqInitializeDataInitialized -= ReceiveInitialDataInitialized;
            
            if (moduleSingletonCtrls?.Any() == true)
            {
                foreach (var ctrl in moduleSingletonCtrls)
                {
                    ctrl.Shutdown();
                }
            }
        }

        /// <summary>
        /// 创建雪橇订单 id。订单格式为:${MACHINEID}_${SUBACCOUNTID}_${SUBUSERID}_${LOGINTIMESTAMP}_{客户端自增}
        /// </summary>
        string ITradeModuleService.GenerateXQOrderId(long subAccountId)
        {
            var loginResp = loginDataService.Value.ProxyLoginResp;
            var loginMachineId = loginResp?.HostingSession?.MachineId ?? 0;
            var loginSubUserId = loginResp?.HostingSession?.SubUserId ?? 0;
            var hostingTimestamp = (loginResp?.HostingTimens ?? 0) / 1000000;
            var localIncreasedId = orderIdIncreaser.RequestIncreasedId();
            return XueQiaoBusinessHelper.CreateXQOrderId(loginMachineId, subAccountId, loginSubUserId, hostingTimestamp, localIncreasedId);
        }

        TaskFactory ITradeModuleService.OrderOperateRequestTaskFactory => _orderOperateReqTaskFactory;

        string ITradeModuleService.GenerateTradeWorkspaceKey()
        {
            var loginResp = loginDataService.Value.ProxyLoginResp;
            var hostingTimestamp = (loginResp?.HostingTimens ?? 0) / 1000000;
            var localIncreasedId = tradeWorkspaceKeyIdIncreaser.RequestIncreasedId();
            return $"{XueQiaoConstants.UserSettingKey_TradeWorkspaceKeyPrefix}{hostingTimestamp}_${localIncreasedId}";
        }

        private UserSubscribeDataTree GetSubscribeDataRunningInThisModule()
        {   
            if (moduleRootViewCtrl == null) return null;
            var subscribeDataGroupsDR = this.subscribeDataGroupsDataRoot;
            if (subscribeDataGroupsDR == null) return null;

            var sharedSubContrs = subscribeContractCtrl.Value.GetSharedGroupKeySubscribeContracts();
            var sharedSubComps = subscribeComposeCtrl.Value.GetSharedGroupKeySubscribeComposes();

            var treeSubContracts = sharedSubContrs?
                .Select(i => new SubscribeContractItem { ContractId = i.ContractId, AddTimestamp = i.AddTimestamp })
                .ToArray();

            var treeContractGroupRelations = sharedSubContrs?
                .Where(i => i.CustomGroupKeys?.Any() == true)
                .Select(i => new SubscribeContractGroupRelation { ContractId = i.ContractId, GroupKeys = i.CustomGroupKeys?.ToArray() })
                .ToArray();
            var treeComposeGroupRelations = sharedSubComps?
                .Where(i => i.CustomGroupKeys?.Any() == true)
                .Select(i => new SubscribeComposeGroupRelation { ComposeId = i.ComposeId, GroupKeys = i.CustomGroupKeys?.ToArray() })
                .ToArray();

            var contractCustomGroups = subscribeDataGroupsDR?.ContractGroups.ToArray()
                .Where(i => i.GroupType == SubscribeDataGroupType.Custom)
                .Select(i => new SubscribeDataCustomGroup { GroupKey = i.GroupKey, GroupName = i.GroupName })
                .ToArray();
            var composeCustomGroups = subscribeDataGroupsDR?.ComposeGroups.ToArray()
                .Where(i => i.GroupType == SubscribeDataGroupType.Custom)
                .Select(i => new SubscribeDataCustomGroup { GroupKey = i.GroupKey, GroupName = i.GroupName })
                .ToArray();

            UserSubscribeDataTree dataTree = new UserSubscribeDataTree
            {
                UserContracts = treeSubContracts,
                UserContractGroupRelations = treeContractGroupRelations,
                UserComposeGroupRelations = treeComposeGroupRelations,
                UserContractListCustomGroups = contractCustomGroups,
                UserComposeListCustomGroups = composeCustomGroups,
            };

            return dataTree;
        }

        private void GetTradeWorkspaceRelatedDataTreesRunningInThisModule(out TradeComponentListColumnInfosDataTree tradeComponentListColumnInfosDataTree,
            out TradeWorkspaceTemplateDataTree tradeWorkspaceTemplateDataTree,
            out WorkspaceWindowTree workspaceWindowTree,
            out IEnumerable<TradeWorkspaceItemTree> workspaceTreeList)
        {
            tradeComponentListColumnInfosDataTree = null;
            tradeWorkspaceTemplateDataTree = null;
            workspaceWindowTree = null;
            workspaceTreeList = null;

            var workspaceDataRoot = this.tradeWorkspaceDataRoot;
            if (workspaceDataRoot == null) return;
            TradeWorkspaceDataConvertHelper.GenerateTradeWorkspaceRelatedDataTrees(
                workspaceDataRoot,
                out tradeComponentListColumnInfosDataTree,
                out tradeWorkspaceTemplateDataTree,
                out workspaceWindowTree,
                out workspaceTreeList);
        }

        private XQComposeOrderEPTDataTree GetXQComposeOrderEPTDataRunningInThisModule()
        {
            if (moduleRootViewCtrl == null) return null;
            var clrEPTs = XQComposeOrderEPTService.ArchivedEPTs.ToArray().Select(dmItem =>
            {
                var clrItem = new ComposeOrderEPT();
                XQComposeOrderEPTHelper.ConfigCLREPTWithDM(clrItem, dmItem);
                return clrItem;
            }).ToArray();

            return new XQComposeOrderEPTDataTree
            {
                EPTs = clrEPTs
            };
        }

        public object GetTradeModuleRootView(Func<ChromeWindowCaptionDataHolder> embedInWindowCaptionDataHolderGetter,
            out Action showAction, out Action closeAction)
        {
            showAction = () => 
            {
                // 显示交易tab 时，刷新关联子账户列表
                relatedSubAccountItemsCtrl.Value.RefreshRelatedSubAccountItemsForce();
            };
            closeAction = null;

            if (this.moduleRootViewCtrl != null) return this.moduleRootViewCtrl.ContentView;
            
            if (tradeWorkspaceDataRoot == null)
                ConfigTradeWorkspaceDataRoot(containerShellService.Value.InitializeData);

            if (subscribeDataGroupsDataRoot == null)
                ConfigSubscribeDataGroupsDataRoot(containerShellService.Value.InitializeData);

            this.moduleRootViewCtrl = moduleRootViewCtrlFactory.CreateExport().Value;
            this.moduleRootViewCtrl.EmbedInWindowCaptionDataHolder = embedInWindowCaptionDataHolderGetter?.Invoke();

            this.moduleRootViewCtrl.Initialize();
            this.moduleRootViewCtrl.Run();
            
            return this.moduleRootViewCtrl.ContentView;
        }

        public TradeWorkspaceDataRoot TradeWorkspaceDataRoot => this.tradeWorkspaceDataRoot;

        public SubscribeDataGroupsDataRoot SubscribeDataGroupsDataRoot => this.subscribeDataGroupsDataRoot;
        

        private void RecvUserIsLogouting(ProxyLoginResp currentLoginResp)
        {
            StopCloudUserSettingsSyncTimer();
            SyncCloudUserSettings(currentLoginResp?.HostingSession?.HostingSession2LandingInfo());
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            orderIdIncreaser.Reset();
            tradeWorkspaceKeyIdIncreaser.Reset();

            // shut down current trade node page controller
            this.moduleRootViewCtrl?.Shutdown();
            this.moduleRootViewCtrl = null;

            this.tradeWorkspaceDataRoot = null;
            this.subscribeDataGroupsDataRoot = null;

            XQComposeOrderEPTService.EPTs.Clear();
            XQComposeOrderEPTService.ArchivedEPTs.Clear();
        }
        
        private void RecvAppShutdown()
        {
            StopCloudUserSettingsSyncTimer();
            SyncCloudUserSettings(loginDataService.Value.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo());
            Shutdown();
        }

        private void ReceiveInitialDataInitialized(InitializeDataRoot initializedData)
        {
            if (initializedData != null)
            {
                AddInitialRelatedSubAccountItemsAndRefreshDatas(initializedData);
                AddInitialSubscribeContracts(initializedData);
                AddInitialSubscribeComposes(initializedData);
                ConfigTradeWorkspaceDataRoot(initializedData);
                ConfigSubscribeDataGroupsDataRoot(initializedData);
                AddInitialXQComposeOrderEPTs(initializedData);
            }
            StartCloudUserSettingsSyncTimer();
        }

        private void ConfigTradeWorkspaceDataRoot(InitializeDataRoot initializedData)
        {
            var dtPackage = initializedData?.LoginUserSettingDataTreePackage;
            var dataRoot = TradeWorkspaceDataConvertHelper.GenerateTradeWorkspaceDataRoot(dtPackage?.TradeComponentListColumnInfosDT,
                dtPackage?.TradeWorkspaceTemplateDT,
                dtPackage?.TradeWorkspaceWindowTree,
                dtPackage?.TradeWorkspaceList);
            TradeWorkspaceDataDisplayHelper.ConfigureWorkspaceDataRootDisplayDefaultIfNeed(dataRoot);
            this.tradeWorkspaceDataRoot = dataRoot;
        }

        private void ConfigSubscribeDataGroupsDataRoot(InitializeDataRoot initializedData)
        {
            var subscribeDataTree = initializedData?.LoginUserSettingDataTreePackage?
                .SubscribeDataTree;

            var contractCustomGroups = subscribeDataTree?.UserContractListCustomGroups?
                .Select(i => new SubscribeDataGroup(SubscribeDataGroupType.Custom, i.GroupKey) { GroupName = i.GroupName })
                .ToArray();
            var composeCustomGroups = subscribeDataTree?.UserComposeListCustomGroups?
                .Select(i => new SubscribeDataGroup(SubscribeDataGroupType.Custom, i.GroupKey) { GroupName = i.GroupName })
                .ToArray();

            var dataRoot = new SubscribeDataGroupsDataRoot
            {
                ContractGroups = contractCustomGroups,
                ComposeGroups = composeCustomGroups
            };
            SubscribeDataDisplayHelper.ConfigFixedGroups(dataRoot);
            this.subscribeDataGroupsDataRoot = dataRoot;
        }
        
        private void AddInitialRelatedSubAccountItemsAndRefreshDatas(InitializeDataRoot initializedData)
        {
            var relatedSubAccountItems = initializedData.LoginUserRelatedSubAccountItems??new HostingSubAccountRelatedItem[] { };
            relatedSubAccountItemsCtrl.Value.RefreshRelatedSubAccountItemsIfNeed(relatedSubAccountItems);
        }

        private void AddInitialSubscribeContracts(InitializeDataRoot initializedData)
        {
            var subscribeDataTree = initializedData?.LoginUserSettingDataTreePackage?
                .SubscribeDataTree;
            if (subscribeDataTree == null) return;

            var subContracts = subscribeDataTree.UserContracts;
            var subContractGroupRelations = subscribeDataTree.UserContractGroupRelations;
            
            var userComposeViews = initializedData.LoginUserAllComposeViews;

            // 添加订阅合约
            if (subContracts?.Any() == true)
            {
                foreach (var contr in subContracts)
                {
                    var contractId = contr.ContractId;
                    subscribeContractCtrl.Value.AddOrUpdateSubscribeContract(contractId, SubscribeContractDataModel.SharedListContractGroupKey,
                        isExist =>
                        {
                            if (isExist) return null;

                            var contr_addTimestamp = contr.AddTimestamp > 0 ? contr.AddTimestamp : (long)DateHelper.NowUnixTimeSpan().TotalSeconds;
                            var contr_customGroupKeys = subContractGroupRelations?.FirstOrDefault(i => i.ContractId == contractId)?.GroupKeys;
                            var contr_isComposeRelated = userComposeViews?
                                .Any(i => i.ComposeGraph?.Legs.Any(leg => leg.SledContractId == contr.ContractId) ?? false)
                                ?? false;

                            return new SubscribeContractUpdateTemplate
                            {
                                AddTimestamp = new Tuple<long>(contr_addTimestamp),
                                CustomGroupKeys = contr_customGroupKeys != null ? new Tuple<IEnumerable<string>>(contr_customGroupKeys):null,
                                IsComposeRelated = new Tuple<bool>(contr_isComposeRelated),
                            };
                        });
                    // 立即订阅
                    subscribeContractCtrl.Value.SubscribeContractQuotationIfNeed(contractId, null);
                }
            }
        }

        private void AddInitialSubscribeComposes(InitializeDataRoot initializedData)
        {
            if (initializedData == null) return;

            var subComposeGroupRelations = initializedData.LoginUserSettingDataTreePackage?
                .SubscribeDataTree?.UserComposeGroupRelations;
            var userComposeViews = initializedData.LoginUserAllComposeViews;

            if (userComposeViews?.Any() == true)
            {
                foreach (var compView in userComposeViews)
                {
                    var composeId = compView.ComposeGraph?.ComposeGraphId;
                    if (composeId == null) continue;
                    subscribeComposeCtrl.Value.AddOrUpdateSubscribeCompose(composeId.Value, SubscribeComposeDataModel.SharedListComposeGroupKey,
                        isExist =>
                        {
                            if (isExist) return null;

                            MarketSubscribeState toSubState = MarketSubscribeState.Unkown;
                            if (compView.UserComposeView?.SubscribeStatus == ClientComposeViewSubscribeStatus.SUBSCRIBED)
                            {
                                toSubState = MarketSubscribeState.Subscribed;
                            }
                            else if (compView.UserComposeView?.SubscribeStatus == ClientComposeViewSubscribeStatus.UNSUBSCRIBED)
                            {
                                toSubState = MarketSubscribeState.Unsubscribed;
                            }

                            var compView_customGroupKeys = subComposeGroupRelations?.FirstOrDefault(i => i.ComposeId == composeId)?.GroupKeys;
                            return new SubscribeComposeUpdateTemplate
                            {
                                SubscribeState = new Tuple<MarketSubscribeState>(toSubState),
                                SubscribeStateMsg = new Tuple<string>(MarketSubscribeStateHelper.DefaultStateMsgForSubscribeState(toSubState)),
                                CustomGroupKeys = compView_customGroupKeys != null ? new Tuple<IEnumerable<string>>(compView_customGroupKeys) : null,
                            };
                        });
                }
            }
        }

        private void AddInitialXQComposeOrderEPTs(InitializeDataRoot initializeData)
        {
            var srcEPTs = initializeData?.LoginUserSettingDataTreePackage?.XQComposeOrderEPTDataTree?.EPTs;
            if (srcEPTs == null) return;

            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                XQComposeOrderEPTService.EPTs.Clear();
                XQComposeOrderEPTService.ArchivedEPTs.Clear();
                foreach (var srcEPT in srcEPTs)
                {
                    if (Enum.TryParse(srcEPT.TemplateType.ToString(), out XQComposeOrderExecParamsSendType templateType))
                    {
                        // Add into EPTs
                        var dmEPT = new XQComposeOrderExecParamsTemplate(srcEPT.Key, templateType)
                        {
                            IsArchived = true,
                            IsInEditMode = false
                        };
                        XQComposeOrderEPTHelper.ConfigDMEPTWithCLR(dmEPT, srcEPT);
                        XQComposeOrderEPTService.EPTs.Add(dmEPT);

                        // Add into ArchivedEPTs
                        var archivedEPT = new XQComposeOrderExecParamsTemplate(srcEPT.Key, templateType)
                        {
                            IsArchived = true,
                            IsInEditMode = false
                        };
                        XQComposeOrderEPTHelper.ConfigDMEPTWithCLR(archivedEPT, srcEPT);
                        XQComposeOrderEPTService.ArchivedEPTs.Add(archivedEPT);
                    }
                }
            });
        }

        private void SyncCloudUserSettings(LandingInfo landingInfo)
        {
            if (landingInfo == null) return;

            AppLog.Debug($"Begin synchronize `Trade` module CloudUserSettings.");
            TradeComponentListColumnInfosDataTree tradeComponentListColumnInfosDataTree = null;
            TradeWorkspaceTemplateDataTree tradeWorkspaceTemplateDataTree = null;
            WorkspaceWindowTree workspaceWindowTree = null;
            IEnumerable<TradeWorkspaceItemTree> workspaceTreeList = null;
            GetTradeWorkspaceRelatedDataTreesRunningInThisModule(out tradeComponentListColumnInfosDataTree,
                out tradeWorkspaceTemplateDataTree,
                out workspaceWindowTree,
                out workspaceTreeList);

            var userSubscribeDataTree = GetSubscribeDataRunningInThisModule();
            var XQComposeOrderEPTData = GetXQComposeOrderEPTDataRunningInThisModule();

            var taskFactory = new TaskFactory();
            var tasks = new List<Task>();

            // update tradeComponentListColumnInfosDataTree
            if (tradeComponentListColumnInfosDataTree != null)
            {
                tasks.Add(taskFactory.StartNew(() =>
                {
                    var content = JsonConvert.SerializeObject(tradeComponentListColumnInfosDataTree);
                    if (content == null) return;
                    userSettingSyncCtrl.Value.UpdateUserSetting(new UserSettingUpdateReq(XueQiaoConstants.UserSettingKey_TradeComponentListColumnInfosData,
                        content, landingInfo), out IInterfaceInteractResponse resp);
                }));
            }

            // update tradeWorkspaceTemplateDataTree
            if (tradeWorkspaceTemplateDataTree != null)
            {
                tasks.Add(taskFactory.StartNew(() =>
                {
                    var content = JsonConvert.SerializeObject(tradeWorkspaceTemplateDataTree);
                    if (content == null) return;
                    userSettingSyncCtrl.Value.UpdateUserSetting(new UserSettingUpdateReq(XueQiaoConstants.UserSettingKey_TradeWorkspaceTemplateData,
                        content, landingInfo), out IInterfaceInteractResponse resp);
                }));
            }

            // update workspaceWindowTree
            if (workspaceWindowTree != null)
            {
                tasks.Add(taskFactory.StartNew(() =>
                {
                    var content = JsonConvert.SerializeObject(workspaceWindowTree);
                    if (content == null) return;
                    userSettingSyncCtrl.Value.UpdateUserSetting(new UserSettingUpdateReq(XueQiaoConstants.UserSettingKey_TradeWorkspaceWindowTree,
                        content, landingInfo), out IInterfaceInteractResponse resp);
                }));
            }

            // update workspaceTreeList
            if (workspaceTreeList != null)
            {
                foreach (var detailTree in workspaceTreeList)
                {
                    var workspaceKey = detailTree.Workspace?.WorkspaceKey;
                    if (string.IsNullOrEmpty(workspaceKey)) continue;
                    tasks.Add(taskFactory.StartNew(() =>
                    {
                        var content = JsonConvert.SerializeObject(detailTree);
                        if (content == null) return;
                        userSettingSyncCtrl.Value.UpdateUserSetting(new UserSettingUpdateReq(workspaceKey,
                            content, landingInfo), out IInterfaceInteractResponse resp);
                    }));
                }
            }

            // update userSubscribeDataTree
            if (userSubscribeDataTree != null)
            {
                tasks.Add(taskFactory.StartNew(() =>
                {
                    var content = JsonConvert.SerializeObject(userSubscribeDataTree);
                    if (content == null) return;
                    userSettingSyncCtrl.Value.UpdateUserSetting(new UserSettingUpdateReq(XueQiaoConstants.UserSettingKey_UserSubscribeData,
                        content, landingInfo), out IInterfaceInteractResponse resp);
                }));
            }

            // update XQComposeOrderEPTData
            if (XQComposeOrderEPTData != null)
            {
                tasks.Add(taskFactory.StartNew(() => 
                {
                    var content = JsonConvert.SerializeObject(XQComposeOrderEPTData);
                    if (content == null) return;
                    userSettingSyncCtrl.Value.UpdateUserSetting(new UserSettingUpdateReq(XueQiaoConstants.UserSettingKey_XQComposeOrderEPTData,
                        content, landingInfo), out IInterfaceInteractResponse resp);
                }));
            }

            var taskArr = tasks.ToArray();
            Task.WaitAll(taskArr);

            AppLog.Debug($"End synchronize `Trade` module CloudUserSettings.");
        }

        private void StartCloudUserSettingsSyncTimer()
        {
            StopCloudUserSettingsSyncTimer();
            cloudUserSettingsSyncTimer = new System.Timers.Timer();
            cloudUserSettingsSyncTimer.Elapsed += SyncUserDataTimer_Elapsed;
            cloudUserSettingsSyncTimer.Interval = 30000; // 30 秒进行一次同步
            cloudUserSettingsSyncTimer.Start();
        }

        private void StopCloudUserSettingsSyncTimer()
        {
            if (cloudUserSettingsSyncTimer != null)
            {
                cloudUserSettingsSyncTimer.Stop();
                cloudUserSettingsSyncTimer.Elapsed -= SyncUserDataTimer_Elapsed;
                cloudUserSettingsSyncTimer.Dispose();
                cloudUserSettingsSyncTimer = null;
            }
        }

        private void SyncUserDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            cloudUserSettingsSyncTimerTaskFactory.StartNew(() => 
            {
                SyncCloudUserSettings(loginDataService.Value.ProxyLoginResp?.HostingSession?.HostingSession2LandingInfo());
            });
        }
    }
}
