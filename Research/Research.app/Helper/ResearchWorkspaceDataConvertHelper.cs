using Research.Interface.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;

namespace Research.app.Helper
{
    
    public static class ResearchWorkspaceDataConvertHelper
    {
        /// <summary>
        /// 生成 ResearchWorkspaceDataRoot
        /// </summary>
        /// <param name="m_workspaceWindowTree">工作空间窗口数据树</param>
        /// <param name="m_workspaceList">工作空间数据列表</param>
        /// <returns></returns>
        public static ResearchWorkspaceDataRoot GenerateResearchWorkspaceDataRoot(
            WorkspaceWindowTree m_workspaceWindowTree,
            IEnumerable<ResearchWorkspaceItemTree> m_workspaceList)
        {
            var dr = new ResearchWorkspaceDataRoot();

            if (m_workspaceWindowTree?.WorkspaceInterTabWindowList != null)
            {
                foreach (var srcWin in m_workspaceWindowTree.WorkspaceInterTabWindowList)
                {
                    var destWin = new InterTabWorkspaceWindowContainer(srcWin.ToTabWorkspaceWindow_DM());
                    if (srcWin.ChildWorkspaceKeys != null)
                    {
                        foreach (var srcWorkspaceKey in srcWin.ChildWorkspaceKeys)
                        {
                            var existWokspaceDetail = m_workspaceList?.FirstOrDefault(i => srcWorkspaceKey == i.Workspace?.WorkspaceKey);
                            if (existWokspaceDetail == null) continue;

                            var nativeWorkspace = existWokspaceDetail.ToTabWorkspace_DM();
                            destWin.WorkspaceListContainer.Workspaces.Add(nativeWorkspace);
                        }
                    }
                    dr.InterTabWorkspaceWindowListContainer.Windows.Add(destWin);
                }
            }

            if (m_workspaceWindowTree?.MainWindowWorkspaceKeyList != null)
            {
                foreach (var srcWorkspaceKey in m_workspaceWindowTree.MainWindowWorkspaceKeyList)
                {
                    var existWokspaceDetail = m_workspaceList?.FirstOrDefault(i => srcWorkspaceKey == i.Workspace?.WorkspaceKey);
                    if (existWokspaceDetail == null) continue;

                    var workspace_dm = existWokspaceDetail.ToTabWorkspace_DM();
                    if (workspace_dm != null)
                    {
                        dr.MainWindowWorkspaceListContainer.Workspaces.Add(workspace_dm);
                    }
                }
            }

            return dr;
        }

        /// <summary>
        /// 生成投研工作空间相关的数据树
        /// </summary>
        /// <param name="wsdr"></param>
        public static void GenerateResearchWorkspaceRelatedDataTrees(ResearchWorkspaceDataRoot wsdr,
            out WorkspaceWindowTree o_workspaceWindowTree,
            out IEnumerable<ResearchWorkspaceItemTree> o_workspaceTreeList)
        {
            o_workspaceWindowTree = null;
            o_workspaceTreeList = null;

            if (wsdr == null) return;

            var tmp_workspaceTreeList = new List<ResearchWorkspaceItemTree>();
            o_workspaceWindowTree = new WorkspaceWindowTree();

            {
                // setup userWorkspaceDataTree.WorkspaceInterTabWindowList
                // 次窗口
                var workspaceInterTabWindowList = new List<XueQiaoFoundation.BusinessResources.Models.TabWorkspaceWindow>();
                var interTabWorkspaceWindows = wsdr.InterTabWorkspaceWindowListContainer.Windows.ToArray();
                foreach (var window in interTabWorkspaceWindows)
                {
                    var srcWinInfo = window.WindowInfo;
                    var srcWorkspaces = window.WorkspaceListContainer.Workspaces.ToArray();
                    var destWin = srcWinInfo.ToTabWorkspaceWindow_M(srcWorkspaces.Select(i => i.WorkspaceKey).ToArray());
                    workspaceInterTabWindowList.Add(destWin);

                    var thisWSList = srcWorkspaces.Select(i => GenerateResearchWorkspaceItemTree(i)).ToArray();
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
                    var destWsDetailTree = GenerateResearchWorkspaceItemTree(srcWs);
                    tmp_workspaceTreeList.Add(destWsDetailTree);
                }
                o_workspaceWindowTree.MainWindowWorkspaceKeyList = srcWorkspaces.Select(i => i.WorkspaceKey).ToArray();
            }

            o_workspaceTreeList = tmp_workspaceTreeList.ToArray();
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

        public static XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ToTabWorkspace_DM(
            this ResearchWorkspaceItemTree detailTree)
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

            if (detailTree.ResearchComponents != null)
            {
                destWs.ResearchComponents.AddRange(detailTree.ResearchComponents.Select(srcComp => srcComp.ToResearchComponent_DM()).ToArray());
            }

            return destWs;
        }
        
        public static ResearchComponent ToResearchComponent_DM(this ResearchComp srcComp)
        {
            var destComp = new ResearchComponent
            {
                Left = srcComp.Left,
                Top = srcComp.Top,
                Width = srcComp.Width,
                Height = srcComp.Height,
                ZIndex = srcComp.ZIndex,
                ComponentType = srcComp.ComponentType,
                UrlCompDetail = srcComp.UrlCompDetail,
            };
            return destComp;
        }
        
        public static ResearchComp ToResearchComponent_M(
            this ResearchComponent srcComp)
        {
            var destComp = new ResearchComp
            {
                Left = srcComp.Left,
                Top = srcComp.Top,
                Width = srcComp.Width,
                Height = srcComp.Height,
                ZIndex = srcComp.ZIndex,
                ComponentType = srcComp.ComponentType,
                UrlCompDetail = srcComp.UrlCompDetail,
            };
            return destComp;
        }
        
        public static ResearchWorkspaceItemTree GenerateResearchWorkspaceItemTree(
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

            var workspaceDataTree = new ResearchWorkspaceItemTree
            {
                Workspace = destWorkspace,
                ResearchComponents = sourceWS.ResearchComponents.Select(i => i.ToResearchComponent_M()).ToArray(),
            };

            return workspaceDataTree;
        }
    }
}
