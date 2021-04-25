using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Contract;
using NativeModel.Trade;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xueqiao.contract.standard;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Helpers;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Shared.Helper.WeakDelegate;

namespace XueQiaoFoundation.Interfaces.Helper
{
    public static class XueQiaoFoundationHelper
    {
        /// <summary>
        /// 设置合约标的类型的数据的合约详情
        /// </summary>
        /// <param name="detailContainer">数据容器</param>
        /// <param name="contractItemTreeQueryController">合约信息查询 controller</param>
        /// <param name="contractDisplayNameFormatType">合约名称格式化类型</param>
        /// <param name="SetupSuccessCallback">设置回调</param>
        public static void SetupTargetContract_ContractDetail(TargetContract_TargetContractDetail detailContainer,
            IContractItemTreeQueryController contractItemTreeQueryController,
            XqContractNameFormatType contractDisplayNameFormatType,
            Action<TargetContract_TargetContractDetail> SetupSuccessCallback = null)
        {
            if (detailContainer == null) return;
            if (contractItemTreeQueryController == null) throw new ArgumentNullException("contractItemTreeQueryController");
            
            Task.Run(() =>
            {
                SyncQueryAndFillContractContainer(detailContainer, contractItemTreeQueryController);

                // 设置显示名称
                SetupDisplayNamesForContractContainer(detailContainer, contractDisplayNameFormatType);
                SetupSuccessCallback?.Invoke(detailContainer);
            });
        }

        /// <summary>
        /// 同步查询合约的详细信息，并填充到 <see cref="detailContainer"/>
        /// </summary>
        /// <param name="detailContainer"></param>
        /// <param name="contractItemTreeQueryCtrl"></param>
        public static void SyncQueryAndFillContractContainer(TargetContract_TargetContractDetail detailContainer,
            IContractItemTreeQueryController contractItemTreeQueryCtrl)
        {
            if (detailContainer == null) return;
            var treeItem = contractItemTreeQueryCtrl.QueryContractItemTree(detailContainer.ContractId, true, true, false);
            if (treeItem != null)
            {
                if (treeItem.Contract != null) detailContainer.ContractDetail = treeItem.Contract;
                if (treeItem.ParentCommodity != null) detailContainer.CommodityDetail = treeItem.ParentCommodity;
                if (treeItem.ParentExchange != null) detailContainer.ExchangeDetail = treeItem.ParentExchange;

                // 查询相关合约，并据此更新 RelatedContractDetails, EngDisplayName, CnDisplayName, TcDisplayName
                var relatedContractIds = detailContainer.ContractDetail?.RelateContractIds;
                if (relatedContractIds?.Any() == true)
                {
                    var relatedContractTrees = contractItemTreeQueryCtrl.QueryTreeItems(relatedContractIds, true, true, false, CancellationToken.None);
                    var relatedContractDetails = new List<TargetContract_TargetContractDetail>();
                    foreach (var relatedContrId in relatedContractIds)
                    {
                        var detail = new TargetContract_TargetContractDetail(relatedContrId);
                        if (relatedContractTrees.TryGetValue(relatedContrId, out ContractItemTree relatedTree))
                        {
                            detail.ContractDetail = relatedTree.Contract;
                            detail.CommodityDetail = relatedTree.ParentCommodity;
                            detail.ExchangeDetail = relatedTree.ParentExchange;
                        }
                        relatedContractDetails.Add(detail);
                    }
                    // 设置 RelatedContractDetails
                    detailContainer.RelatedContractDetails = new ObservableCollection<TargetContract_TargetContractDetail>(relatedContractDetails);
                }
            }
        }

        /// <summary>
        /// 设置显示名称的各个字段。EngDisplayName, CnDisplayName, TcDisplayName
        /// </summary>
        /// <param name="detailContainer"></param>
        /// <param name="contractNameFormatType"></param>
        public static void SetupDisplayNamesForContractContainer(TargetContract_TargetContractDetail detailContainer, XqContractNameFormatType contractNameFormatType)
        {
            if (detailContainer == null) return;
            var selfCommodity = detailContainer.CommodityDetail;
            if (selfCommodity == null) return;

            var relatedContractDetailTuples = detailContainer.RelatedContractDetails?
                    .Select(i => new Tuple<NativeCommodity, NativeContract>(i.CommodityDetail, i.ContractDetail)).ToArray();

            var EngDisplayName = "";
            var CnDisplayName = "";
            var TcDisplayName = "";
            if (selfCommodity.SledCommodityType == SledCommodityType.SPREAD_MONTH.GetHashCode())
            {
                // 跨期
                var rootContractDetailTuple = new Tuple<NativeCommodity, NativeContract>(detailContainer.CommodityDetail, detailContainer.ContractDetail);
                EngDisplayName = XueQiaoBusinessHelper.FormatArbitrageContractNameOfSameRootContract(contractNameFormatType,
                    XqAppLanguages.ENG,
                    rootContractDetailTuple,
                    relatedContractDetailTuples);
                CnDisplayName = XueQiaoBusinessHelper.FormatArbitrageContractNameOfSameRootContract(contractNameFormatType,
                    XqAppLanguages.CN,
                    rootContractDetailTuple,
                    relatedContractDetailTuples);
                TcDisplayName = XueQiaoBusinessHelper.FormatArbitrageContractNameOfSameRootContract(contractNameFormatType,
                    XqAppLanguages.TC,
                    rootContractDetailTuple,
                    relatedContractDetailTuples);
            }
            else if (selfCommodity.SledCommodityType == SledCommodityType.SPREAD_COMMODITY.GetHashCode())
            {
                // 跨品种
                EngDisplayName = XueQiaoBusinessHelper.FormatArbitrageContractNameOfDiffRootContract(contractNameFormatType,
                    XqAppLanguages.ENG,
                    relatedContractDetailTuples);
                CnDisplayName = XueQiaoBusinessHelper.FormatArbitrageContractNameOfDiffRootContract(contractNameFormatType,
                    XqAppLanguages.CN,
                    relatedContractDetailTuples);
                TcDisplayName = XueQiaoBusinessHelper.FormatArbitrageContractNameOfDiffRootContract(contractNameFormatType,
                    XqAppLanguages.TC,
                    relatedContractDetailTuples);
            }
            else
            {
                var rootContractDetailTuple = new Tuple<NativeCommodity, NativeContract>(detailContainer.CommodityDetail, detailContainer.ContractDetail);
                EngDisplayName = XueQiaoBusinessHelper.FormatContractName(contractNameFormatType,
                    XqAppLanguages.ENG,
                    rootContractDetailTuple);
                CnDisplayName = XueQiaoBusinessHelper.FormatContractName(contractNameFormatType,
                    XqAppLanguages.CN,
                    rootContractDetailTuple);
                TcDisplayName = XueQiaoBusinessHelper.FormatContractName(contractNameFormatType,
                    XqAppLanguages.TC,
                    rootContractDetailTuple);
            }

            detailContainer.EngDisplayName = EngDisplayName;
            detailContainer.CnDisplayName = CnDisplayName;
            detailContainer.TcDisplayName = TcDisplayName;
        }

        /// <summary>
        /// 设置商品详情容器
        /// </summary>
        /// <param name="detailContainer">商品详情容器</param>
        /// <param name="contractItemTreeQueryController"></param>
        /// <param name="isAsync">是否异步</param>
        public static void SetupCommodityDetailContainer(CommodityDetailContainer detailContainer,
            IContractItemTreeQueryController contractItemTreeQueryController,
            bool isAsync)
        {
            if (detailContainer == null) return;

            var queryAndSetupContainer = new Action(() =>
            {
                var itemTree = contractItemTreeQueryController.QueryCommodityItemTree(detailContainer.CommodityId, true, false);
                if (itemTree?.Commodity != null) detailContainer.CommodityDetail = itemTree?.Commodity;
                if (itemTree?.ParentExchange != null) detailContainer.ExchangeDetail = itemTree?.ParentExchange;
            });

            if (isAsync)
            {
                Task.Run(() =>
                {
                    queryAndSetupContainer();
                });
            }
            else
            {
                queryAndSetupContainer();
            }
        }

        /// <summary>
        /// 获取雪橇组合
        /// </summary>
        /// <param name="composeGraphId"></param>
        /// <param name="composeGraphCacheController"></param>
        /// <param name="composeGraphQueryController"></param>
        /// <param name="userComposeViewCacheController"></param>
        /// <param name="LoadedCallback"></param>
        public static void LoadXQComposeGraph(long composeGraphId,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController,
            Action<NativeComposeGraph> LoadedCallback = null)
        {
            Task.Run(() =>
            {
                var tarCompose = QueryXQComposeGraph(composeGraphId,
                        composeGraphCacheController,
                        composeGraphQueryController,
                        userComposeViewCacheController);
                LoadedCallback?.Invoke(tarCompose);
            });
        }

        /// <summary>
        /// 同步获取雪橇组合信息。如果存在本地缓存，则先从缓存中查询
        /// </summary>
        /// <param name="composeGraphId"></param>
        /// <param name="composeGraphCacheController"></param>
        /// <param name="composeGraphQueryController"></param>
        /// <param name="userComposeViewCacheController"></param>
        /// <returns></returns>
        public static NativeComposeGraph QueryXQComposeGraph(long composeGraphId,
            IComposeGraphCacheController composeGraphCacheController,
            IComposeGraphQueryController composeGraphQueryController,
            IUserComposeViewCacheController userComposeViewCacheController)
        {
            if (composeGraphCacheController == null) throw new ArgumentNullException("composeCacheController");
            if (composeGraphQueryController == null) throw new ArgumentNullException("composeQueryController");
            if (userComposeViewCacheController == null) throw new ArgumentNullException("userComposeViewCacheController");

            var tarCompose = composeGraphCacheController.GetCache(composeGraphId);
            if (tarCompose != null) return tarCompose;

            tarCompose = userComposeViewCacheController.AllCaches().Values.FirstOrDefault(i => i.ComposeGraph?.ComposeGraphId == composeGraphId)?.ComposeGraph;
            if (tarCompose != null) return tarCompose;

            tarCompose = composeGraphQueryController.QueryComposeGraph(composeGraphId)?.CorrectResult;
            return tarCompose;
        }

        /// <summary>
        /// 设置组合标的类型的数据的组合详情
        /// </summary>
        /// <param name="detailContainer">数据容器</param>
        /// <param name="legDisplayNameFormatType">组合腿名称显示格式</param>
        /// <param name="SetupSuccessCallback">所有详情数据都设置了的回调</param>
        public static void SetupTargetCompose_ComposeDetail(TargetCompose_ComposeDetail detailContainer,
            IComposeGraphCacheController composeGraphCacheCtrl,
            IComposeGraphQueryController composeGraphQueryCtrl,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IContractItemTreeQueryController contractItemTreeQueryCtrl,
            XqContractNameFormatType legDisplayNameFormatType,
            Action<TargetCompose_ComposeDetail> SetupSuccessCallback = null)
        {
            if (detailContainer == null) return;

            Debug.Assert(composeGraphCacheCtrl != null);
            Debug.Assert(composeGraphQueryCtrl != null);
            Debug.Assert(userComposeViewCacheCtrl != null);
            Debug.Assert(contractItemTreeQueryCtrl != null);

            var composeGraphId = detailContainer.ComposeId;
            var LoadedXqComposeGraph = new Action<NativeComposeGraph>(loadedCompose =>
            {
                detailContainer.BasicComposeGraph = loadedCompose;
                if (loadedCompose?.Legs != null)
                {
                    var legDetailItems = new List<ComposeLegDetail>();

                    var tsks = new List<Task>();
                    foreach (var basicLeg in loadedCompose.Legs)
                    {
                        var legDetail = new ComposeLegDetail(basicLeg);
                        legDetailItems.Add(legDetail);
                        tsks.Add(Task.Run(() =>
                        {
                            var legContainer = legDetail.LegDetailContainer;
                            XueQiaoFoundationHelper.SyncQueryAndFillContractContainer(legContainer, contractItemTreeQueryCtrl);
                            XueQiaoFoundationHelper.SetupDisplayNamesForContractContainer(legContainer, legDisplayNameFormatType);
                        }));
                    }
                    Task.WaitAll(tsks.ToArray());

                    detailContainer.DetailLegs = new ObservableCollection<ComposeLegDetail>(legDetailItems);
                }
                else
                {
                    detailContainer.DetailLegs = null;
                }
            });

            LoadXQComposeGraph(composeGraphId,
                composeGraphCacheCtrl,
                composeGraphQueryCtrl,
                userComposeViewCacheCtrl,
                _composeGraph =>
                {
                    LoadedXqComposeGraph(_composeGraph);
                    SetupSuccessCallback?.Invoke(detailContainer);
                });
        }


        /// <summary>
        ///  获取用户的组合视图
        /// </summary>
        /// <param name="composeGraphId">组合id</param>
        /// <param name="userComposeViewCacheCtrl"></param>
        /// <param name="userComposeViewQueryCtrl"></param>
        /// <param name="ignoreCache">是否忽略本地缓存</param>
        /// <param name="queryHistory">是否查询历史记录</param>
        /// <returns></returns>
        public static NativeComposeViewDetail QueryUserComposeView(long composeGraphId,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IUserComposeViewQueryController userComposeViewQueryCtrl,
            bool ignoreCache,
            bool queryHistory)
        {
            NativeComposeViewDetail queriedView = null;
            bool needQueryFromCloud = true;
            if (!ignoreCache)
            {
                var cacheKey = new UserComposeViewCacheKey(composeGraphId);
                var cachedComposeView = userComposeViewCacheCtrl.GetCache(cacheKey);
                if (cachedComposeView != null)
                {
                    queriedView = cachedComposeView;
                    needQueryFromCloud = false;
                }
            }

            if (!needQueryFromCloud)
                return queriedView;
            
            if (queryHistory)
            {
                queriedView = userComposeViewQueryCtrl.QueryHistoryComposeView(composeGraphId)?.CorrectResult;
            }
            else
            {
                queriedView = userComposeViewQueryCtrl.QueryCurrentComposeView(composeGraphId)?.CorrectResult;
            }
            return queriedView;
        }

        /// <summary>
        /// 加载用户的组合视图
        /// </summary>
        /// <param name="composeGraphId">组合id</param>
        /// <param name="userComposeViewCacheCtrl"></param>
        /// <param name="userComposeViewQueryCtrl"></param>
        /// <param name="ignoreCache">是否忽略本地缓存</param>
        /// <param name="queryHistory">是否查询历史记录</param>
        /// <param name="loadedCallback">查询结果回调</param>
        public static void LoadUserComposeView(
            long composeGraphId,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IUserComposeViewQueryController userComposeViewQueryCtrl,
            bool ignoreCache,
            bool queryHistory,
            Action<NativeComposeViewDetail> loadedCallback)
        {
            var LoadedComposeView = new Action<NativeComposeViewDetail>(_loadedResult =>
            {
                loadedCallback?.Invoke(_loadedResult?.UserComposeView?.ComposeGraphId == composeGraphId ? _loadedResult : null);
            });

            Task.Run(() =>
            {
                bool needQueryFromCloud = true;
                if (!ignoreCache)
                {
                    var cacheKey = new UserComposeViewCacheKey(composeGraphId);
                    var cachedComposeView = userComposeViewCacheCtrl.GetCache(cacheKey);
                    if (cachedComposeView != null)
                    {
                        LoadedComposeView(cachedComposeView);
                        needQueryFromCloud = false;
                    }
                }

                if (!needQueryFromCloud) return;

                var queryHandler = new Action<IInterfaceInteractResponse<NativeComposeViewDetail>>(resp =>
                {
                    var queriedComposeView = resp?.CorrectResult;
                    LoadedComposeView(queriedComposeView);
                });
                var queryHandlerRefer = new ActionDelegateReference<IInterfaceInteractResponse<NativeComposeViewDetail>>(queryHandler, true);

                if (queryHistory)
                {
                    userComposeViewQueryCtrl.QueryHistoryComposeView(composeGraphId, queryHandlerRefer);
                }
                else
                {
                    userComposeViewQueryCtrl.QueryCurrentComposeView(composeGraphId, queryHandlerRefer);
                }
            });
        }

        /// <summary>
        /// 设置用户的组合视图容器
        /// </summary>
        /// <param name="userComposeViewContainer"></param>
        /// <param name="userComposeViewCacheCtrl"></param>
        /// <param name="userComposeViewQueryCtrl"></param>
        /// <param name="ignoreCache">是否忽略本地缓存</param>
        /// <param name="queryHistory">是否查询历史记录</param>
        /// <param name="setupSuccessCallback"></param>
        public static void SetupUserComposeView(
            UserComposeViewContainer userComposeViewContainer,
            IUserComposeViewCacheController userComposeViewCacheCtrl,
            IUserComposeViewQueryController userComposeViewQueryCtrl,
            bool ignoreCache,
            bool queryHistory,
            Action<UserComposeViewContainer> setupSuccessCallback = null)
        {
            if (userComposeViewContainer == null) return;
            LoadUserComposeView(userComposeViewContainer.ComposeGraphId,
                userComposeViewCacheCtrl, userComposeViewQueryCtrl,
                ignoreCache, queryHistory,
                _detail =>
                {
                    userComposeViewContainer.UserComposeView = _detail?.UserComposeView;
                    setupSuccessCallback?.Invoke(userComposeViewContainer);
                });
        }
    }
}
