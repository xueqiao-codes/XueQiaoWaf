using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    public static class TradeWorkspaceDataConvertHelper
    {
        /// <summary>
        /// 生成交易工作空间相关的数据树
        /// </summary>
        /// <param name="workspaceDataRoot"></param>
        /// <param name="tradeComponentListColumnInfosDataTree"></param>
        /// <param name="tradeWorkspaceTemplateDataTree"></param>
        /// <param name="workspaceWindowTree"></param>
        /// <param name="workspaceTreeList"></param>
        public static void GenerateTradeWorkspaceRelatedDataTrees(this TradeWorkspaceDataRoot wsdr,
            out XueQiaoFoundation.BusinessResources.Models.TradeComponentListColumnInfosDataTree o_tradeComponentListColumnInfosDataTree,
            out XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceTemplateDataTree o_tradeWorkspaceTemplateDataTree,
            out XueQiaoFoundation.BusinessResources.Models.WorkspaceWindowTree o_workspaceWindowTree,
            out IEnumerable<XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceItemTree> o_workspaceTreeList)
        {
            o_tradeComponentListColumnInfosDataTree = null;
            o_tradeWorkspaceTemplateDataTree = null;
            o_workspaceWindowTree = null;
            o_workspaceTreeList = null;

            if (wsdr == null) return;

            // setup tradeComponentListColumnInfosDataTree
            o_tradeComponentListColumnInfosDataTree = new XueQiaoFoundation.BusinessResources.Models.TradeComponentListColumnInfosDataTree();
            o_tradeComponentListColumnInfosDataTree.GlobalAppliedContractListDisplayColumns = wsdr.GlobalAppliedContractListDisplayColumns;
            o_tradeComponentListColumnInfosDataTree.GlobalAppliedComposeListDisplayColumns = wsdr.GlobalAppliedComposeListDisplayColumns;
            o_tradeComponentListColumnInfosDataTree.GlobalAppliedOrderListEntrustedDisplayColumns = wsdr.GlobalAppliedOrderListEntrustedDisplayColumns;
            o_tradeComponentListColumnInfosDataTree.GlobalAppliedOrderListParkedDisplayColumns = wsdr.GlobalAppliedOrderListParkedDisplayColumns;
            o_tradeComponentListColumnInfosDataTree.GlobalAppliedOrderListConditionDisplayColumns = wsdr.GlobalAppliedOrderListConditionDisplayColumns;
            o_tradeComponentListColumnInfosDataTree.GlobalAppliedTradeListDisplayColumns = wsdr.GlobalAppliedTradeListDisplayColumns;

            // setup tradeWorkspaceTemplateDataTree
            o_tradeWorkspaceTemplateDataTree = new XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceTemplateDataTree();
            o_tradeWorkspaceTemplateDataTree.Templates = wsdr.TradeWorkspaceTemplateListContainer.Templates.ToArray()
                .Select(i => i.ToTradeTabWorkspaceTemplate_M())
                .ToArray();

            // setup userWorkspaceDataTree and tabWorkspaceDataTrees
            var tmp_workspaceTreeList = new List<XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceItemTree>();
            o_workspaceWindowTree = new XueQiaoFoundation.BusinessResources.Models.WorkspaceWindowTree();
            
            {
                // setup userWorkspaceDataTree.WorkspaceInterTabWindowList
                // 次窗口
                var workspaceInterTabWindowList = new List<XueQiaoFoundation.BusinessResources.Models.TabWorkspaceWindow>();
                var interTabWorkspaceWindows = wsdr.InterTabWorkspaceWindowListContainer.Windows.ToArray();
                foreach (var window in interTabWorkspaceWindows)
                {
                    var srcWinInfo = window.WindowInfo;
                    var srcWorkspaces = window.WorkspaceListContainer.Workspaces.ToArray();
                    var destWin = srcWinInfo.ToTabWorkspaceWindow_M(srcWorkspaces.Select(i=>i.WorkspaceKey).ToArray());
                    workspaceInterTabWindowList.Add(destWin);

                    var thisWSList = srcWorkspaces.Select(i => GenerateTradeWorkspaceItemTree(i)).ToArray();
                    tmp_workspaceTreeList.AddRange(thisWSList);
                }

                o_workspaceWindowTree.WorkspaceInterTabWindowList = workspaceInterTabWindowList.ToArray();
            }

            {
                // setup userWorkspaceDataTree.MainWindowWorkspaceList
                // 主窗口
                var srcWorkspaces = wsdr.MainWindowWorkspaceListContainer.Workspaces.ToArray();
                foreach (var srcWs in srcWorkspaces)
                {
                    var destWsDetailTree = GenerateTradeWorkspaceItemTree(srcWs);
                    tmp_workspaceTreeList.Add(destWsDetailTree);
                }
                o_workspaceWindowTree.MainWindowWorkspaceKeyList = srcWorkspaces.Select(i => i.WorkspaceKey).ToArray();
            }

            // setup tabWorkspaceDataTrees
            o_workspaceTreeList = tmp_workspaceTreeList.ToArray();
        }


        public static XueQiaoFoundation.BusinessResources.Models.TabWorkspaceWindow ToTabWorkspaceWindow_M(
            this XueQiaoFoundation.BusinessResources.DataModels.TabWorkspaceWindow source,
            string[] childWorkspaceKeys)
        {
            var dest = new XueQiaoFoundation.BusinessResources.Models.TabWorkspaceWindow();
            dest.Left = source.Left;
            dest.Top = source.Top;
            dest.Width = source.Width;
            dest.Height = source.Height;
            dest.IsMaximized = source.IsMaximized;
            dest.ChildWorkspaceKeys = childWorkspaceKeys;
            return dest;
        }


        public static XueQiaoFoundation.BusinessResources.Models.TradeComp ToTradeComponent_M(
            this XueQiaoFoundation.BusinessResources.DataModels.TradeComponent srcComp)
        {
            var destComp = new XueQiaoFoundation.BusinessResources.Models.TradeComp
            {
                Left = srcComp.Left,
                Top = srcComp.Top,
                Width = srcComp.Width,
                Height = srcComp.Height,
                ZIndex = srcComp.ZIndex,
                ComponentType = srcComp.ComponentType,
                IsLocked = srcComp.IsLocked,
                ComponentDescTitle = srcComp.ComponentDescTitle,
                SubscribeDataContainerComponentDetail = srcComp.SubscribeDataContainerComponentDetail,
                PlaceOrderContainerComponentDetail = srcComp.PlaceOrderContainerComponentDetail,
                AccountContainerComponentDetail = srcComp.AccountContainerComponentDetail,
            };
            return destComp;
        }

        public static XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceItemTree GenerateTradeWorkspaceItemTree(
            XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace sourceWS)
        {
            var destWorkspace = new XueQiaoFoundation.BusinessResources.Models.TabWorkspace
            {
                WorkspaceKey = sourceWS.WorkspaceKey,
                WorkspaceType = sourceWS.WorkspaceType,
                Name = sourceWS.Name,
                WorkspaceDesc = sourceWS.WorkspaceDesc,
                Order = sourceWS.Order,
                IsSelected = sourceWS.IsSelected,
                SubAccountId = sourceWS.SubAccountId,
            };

            var workspaceDataTree = new XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceItemTree
            {
                Workspace = destWorkspace,
                TradeComponents = sourceWS.TradeComponents.Select(i => i.ToTradeComponent_M()).ToArray(),
            };

            return workspaceDataTree;
        }

        public static XueQiaoFoundation.BusinessResources.Models.TradeTabWorkspaceTemplate ToTradeTabWorkspaceTemplate_M(
            this XueQiaoFoundation.BusinessResources.DataModels.TradeTabWorkspaceTemplate source)
        {
            var dest = new XueQiaoFoundation.BusinessResources.Models.TradeTabWorkspaceTemplate
            {
                TemplateId = source.TemplateId,
                TemplateName = source.TemplateName,
                TemplateDesc = source.TemplateDesc,
                CreateTimestamp = source.CreateTimestamp,
                LastUpdateTimestamp = source.LastUpdateTimestamp,
                ChildComponents = source.ChildComponents
            };

            return dest;
        }



        public static TradeWorkspaceDataRoot GenerateTradeWorkspaceDataRoot(
            XueQiaoFoundation.BusinessResources.Models.TradeComponentListColumnInfosDataTree tradeComponentListColumnInfosDataTree,
            XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceTemplateDataTree tradeWorkspaceTemplateDataTree,
            XueQiaoFoundation.BusinessResources.Models.WorkspaceWindowTree tradeWorkspaceWindowTree,
            IEnumerable<XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceItemTree> tradeWorkspaceList)
        {
            var destDataRoot = new TradeWorkspaceDataRoot();

            // setup list column infos
            destDataRoot.GlobalAppliedContractListDisplayColumns = tradeComponentListColumnInfosDataTree?.GlobalAppliedContractListDisplayColumns;
            destDataRoot.GlobalAppliedComposeListDisplayColumns = tradeComponentListColumnInfosDataTree?.GlobalAppliedComposeListDisplayColumns;
            destDataRoot.GlobalAppliedOrderListEntrustedDisplayColumns = tradeComponentListColumnInfosDataTree?.GlobalAppliedOrderListEntrustedDisplayColumns;
            destDataRoot.GlobalAppliedOrderListParkedDisplayColumns = tradeComponentListColumnInfosDataTree?.GlobalAppliedOrderListParkedDisplayColumns;
            destDataRoot.GlobalAppliedOrderListConditionDisplayColumns = tradeComponentListColumnInfosDataTree?.GlobalAppliedOrderListConditionDisplayColumns;
            destDataRoot.GlobalAppliedTradeListDisplayColumns = tradeComponentListColumnInfosDataTree?.GlobalAppliedTradeListDisplayColumns;

            // setup workspace templates
            if (tradeWorkspaceTemplateDataTree?.Templates != null)
            {
                destDataRoot.TradeWorkspaceTemplateListContainer.Templates
                    .AddRange(tradeWorkspaceTemplateDataTree.Templates.Select(i=>i.ToNativeTradeTabWorkspaceTemplate()));
            }

            // setup workspace datas
            if (tradeWorkspaceWindowTree?.WorkspaceInterTabWindowList != null)
            {
                foreach (var srcWin in tradeWorkspaceWindowTree.WorkspaceInterTabWindowList)
                {
                    var destWin = new XueQiaoFoundation.BusinessResources.DataModels.InterTabWorkspaceWindowContainer(srcWin.ToTabWorkspaceWindow_DM());
                    if (srcWin.ChildWorkspaceKeys != null)
                    {
                        foreach (var srcWorkspaceKey in srcWin.ChildWorkspaceKeys)
                        {
                            var existWokspaceDetail = tradeWorkspaceList?.FirstOrDefault(i => srcWorkspaceKey == i.Workspace?.WorkspaceKey);
                            if (existWokspaceDetail == null) continue;

                            var workspace_dm = existWokspaceDetail.ToTabWorkspace_DM();
                            if (workspace_dm != null)
                            {
                                destWin.WorkspaceListContainer.Workspaces.Add(workspace_dm);
                            }
                        }
                    }
                    destDataRoot.InterTabWorkspaceWindowListContainer.Windows.Add(destWin);
                }
            }

            if (tradeWorkspaceWindowTree?.MainWindowWorkspaceKeyList != null)
            {
                foreach (var srcWorkspaceKey in tradeWorkspaceWindowTree.MainWindowWorkspaceKeyList)
                {
                    var existWokspaceDetail = tradeWorkspaceList?.FirstOrDefault(i => srcWorkspaceKey == i.Workspace?.WorkspaceKey);
                    if (existWokspaceDetail == null) continue;

                    var workspace_dm = existWokspaceDetail.ToTabWorkspace_DM();
                    if (workspace_dm != null)
                    {
                        destDataRoot.MainWindowWorkspaceListContainer.Workspaces.Add(workspace_dm);
                    }
                }
            }

            return destDataRoot;
        }

        public static XueQiaoFoundation.BusinessResources.DataModels.TabWorkspaceWindow ToTabWorkspaceWindow_DM(
             this XueQiaoFoundation.BusinessResources.Models.TabWorkspaceWindow m_source)
        {
            var dest_dm = new XueQiaoFoundation.BusinessResources.DataModels.TabWorkspaceWindow();
            dest_dm.Left = m_source.Left;
            dest_dm.Top = m_source.Top;
            dest_dm.Width = m_source.Width;
            dest_dm.Height = m_source.Height;
            dest_dm.IsMaximized = m_source.IsMaximized;
            return dest_dm;
        }


        public static XueQiaoFoundation.BusinessResources.DataModels.TradeComponent ToTradeComponent_DM(
            this XueQiaoFoundation.BusinessResources.Models.TradeComp srcComp)
        {
            var destComp = new XueQiaoFoundation.BusinessResources.DataModels.TradeComponent
            {
                Left = srcComp.Left,
                Top = srcComp.Top,
                Width = srcComp.Width,
                Height = srcComp.Height,
                ZIndex = srcComp.ZIndex,
                ComponentType = srcComp.ComponentType,
                IsLocked = srcComp.IsLocked,
                ComponentDescTitle = srcComp.ComponentDescTitle,
                SubscribeDataContainerComponentDetail = srcComp.SubscribeDataContainerComponentDetail,
                PlaceOrderContainerComponentDetail = srcComp.PlaceOrderContainerComponentDetail,
                AccountContainerComponentDetail = srcComp.AccountContainerComponentDetail,
            };

            return destComp;
        }

        public static XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ToTabWorkspace_DM(
            this XueQiaoFoundation.BusinessResources.Models.TradeWorkspaceItemTree detailTree)
        {
            var srcWs = detailTree.Workspace;
            if (srcWs == null)
            {
                return null;
            }

            var destWs = new XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace(srcWs.WorkspaceKey)
            {
                WorkspaceType = srcWs.WorkspaceType,
                Name = srcWs.Name,
                WorkspaceDesc = srcWs.WorkspaceDesc,
                Order = srcWs.Order,
                IsSelected = srcWs.IsSelected,
                SubAccountId = srcWs.SubAccountId
            };

            if (detailTree.TradeComponents != null)
            {
                destWs.TradeComponents.AddRange(detailTree.TradeComponents.Select(srcComp => srcComp.ToTradeComponent_DM()).ToArray());
            }

            return destWs;
        }

        public static XueQiaoFoundation.BusinessResources.DataModels.TradeTabWorkspaceTemplate ToNativeTradeTabWorkspaceTemplate(this XueQiaoFoundation.BusinessResources.Models.TradeTabWorkspaceTemplate source)
        {
            var dest = new XueQiaoFoundation.BusinessResources.DataModels.TradeTabWorkspaceTemplate(source.TemplateId)
            {
                TemplateName = source.TemplateName,
                TemplateDesc = source.TemplateDesc,
                CreateTimestamp = source.CreateTimestamp,
                LastUpdateTimestamp = source.LastUpdateTimestamp,
                ChildComponents = source.ChildComponents,
            };
            return dest;
        }
    }
}
