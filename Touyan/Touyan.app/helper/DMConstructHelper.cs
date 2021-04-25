using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.app.datamodel;
using Touyan.Interface.datamodel;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.app.helper
{
    public static class DMConstructHelper
    {
        /// <summary>
        /// 生成图表收藏文件夹树
        /// </summary>
        /// <param name="allFavorItems">所有收藏项</param>
        /// <param name="rootFavorFolderId">根收藏文件夹 id</param>
        /// <param name="exclueChartType">是否排除 Chart 类型的项</param>
        /// <returns></returns>
        public static ChartFavoriteListTreeNode_Folder GenerateFavoriteTreeNodeByRootFolderId(ChartFavoriteListItem[] allFavorItems,
            long rootFavorFolderId, params ChartFolderListItemType[] excludeItemTypes)
        {
            if (allFavorItems == null) allFavorItems = new ChartFavoriteListItem[] { };
            if (excludeItemTypes == null) excludeItemTypes = new ChartFolderListItemType[] { };

            var rootItem = allFavorItems.FirstOrDefault(i => i.FavoriteId == rootFavorFolderId
                && i is ChartFavoriteListItem_Folder) as ChartFavoriteListItem_Folder;
            if (rootItem == null)
            {
                return null;
            }

            var node = new ChartFavoriteListTreeNode_Folder(rootItem.FolderId)
            { FavoriteData = rootItem };

            // 设置子文件夹
            var childNodes = allFavorItems.Where(i => i.ParentFavoriteFolderId == rootItem.FolderId
                && !excludeItemTypes.Contains(i.ItemType)).ToArray();
            foreach (var child in childNodes)
            {
                ChartFavoriteListTreeNodeBase childNode = null;
                if (child is ChartFavoriteListItem_Folder favorFolder)
                {
                    childNode = GenerateFavoriteTreeNodeByRootFolderId(allFavorItems, favorFolder.FolderId, excludeItemTypes);
                }
                else if (child is ChartFavoriteListItem_Chart favorChart)
                {
                    childNode = new ChartFavoriteListTreeNode_Chart(favorChart.FavoriteId, favorChart.ChartId)
                    { FavoriteData = favorChart };
                }

                if (childNode != null)
                {
                    node.Children.Add(childNode);
                }
            }

            return node;
        }

        /// <summary>
        /// 生成图表文件夹树
        /// </summary>
        /// <param name="allFolderList">所有文件夹</param>
        /// <param name="rootFolderId">根文件夹 id</param>
        /// <returns></returns>
        public static ChartFolderListTreeNode_Folder GenerateChartFolderTreeNodeByRootFolderId(ChartFolder[] allFolderList,
            long rootFolderId)
        {
            if (allFolderList == null) allFolderList = new ChartFolder[] { };

            var rootFolder = allFolderList.FirstOrDefault(i => i.FolderId == rootFolderId);
            if (rootFolder == null)
            {
                return null;
            }

            var node = new ChartFolderListTreeNode_Folder(rootFolder.FolderId, rootFolder.ParentFolderId)
            { Folder = rootFolder };

            // 设置子文件夹
            var childFolders = allFolderList.Where(i => i.ParentFolderId == rootFolder.FolderId).ToArray();
            foreach (var child in childFolders)
            {
                var childNode = GenerateChartFolderTreeNodeByRootFolderId(allFolderList, child.FolderId);
                if (childNode != null)
                {
                    node.Children.Add(childNode);
                }
            }

            return node;
        }

        /// <summary>
        /// 从所有图表文件夹中获得实际意义上的根文件夹
        /// </summary>
        /// <param name="allFavorItems"></param>
        /// <returns></returns>
        public static ChartFolder GetMeaningfulRootChartFolder(IEnumerable<ChartFolder> allFolders)
        {
            return allFolders?.FirstOrDefault(i => i.ParentFolderId == 0);
        }

        /// <summary>
        /// 从所有图表收藏项中获得实际意义上的根文件夹
        /// </summary>
        /// <param name="allFavorItems"></param>
        /// <returns></returns>
        public static ChartFavoriteListItem_Folder GetMeaningfulRootChartFavoriteItem(IEnumerable<ChartFavoriteListItem> allFavorItems)
        {
            return allFavorItems?.FirstOrDefault(i => i.ParentFavoriteFolderId == 0 && i is ChartFavoriteListItem_Folder) as ChartFavoriteListItem_Folder;
        }

        /// <summary>
        /// 从收藏夹树中获取某个指定收藏夹
        /// </summary>
        /// <param name="folderFavorId">指定收藏夹 id</param>
        /// <param name="rootNode">根 node</param>
        /// <returns></returns>
        public static ChartFavoriteListTreeNode_Folder GetFavoriteFolderNodeFromTree(long folderFavorId,
            ChartFavoriteListTreeNode_Folder rootNode)
        {
            if (rootNode == null) return null;

            if (rootNode.FolderId == folderFavorId) return rootNode;

            foreach (var childNode in rootNode.Children)
            {
                if (childNode is ChartFavoriteListTreeNode_Folder folderChildNode)
                {
                    var tarNode = GetFavoriteFolderNodeFromTree(folderFavorId, folderChildNode);
                    if (tarNode != null)
                    {
                        return tarNode;
                    }
                }
            }

            return null;
        }

        public static ChartFavoriteListTreeNodeBase GetFavoriteNodeFromTree(ChartFolderListItemType favorItemType,
            long favorId,
            ChartFavoriteListTreeNode_Folder rootNode)
        {
            ChartFavoriteListTreeNodeBase tarNode = null;
            if (favorItemType == ChartFolderListItemType.Folder)
            {
                tarNode = DMConstructHelper.GetFavoriteFolderNodeFromTree(favorId, rootNode);
            }
            else if (favorItemType == ChartFolderListItemType.Chart)
            {
                tarNode = DMConstructHelper.GetFavoriteChartNodeFromTree(favorId, rootNode);
            }
            return tarNode;
        }

        /// <summary>
        /// 从收藏夹树中获取某个指定收藏图表
        /// </summary>
        /// <param name="chartFavorId">指定收藏图表 id</param>
        /// <param name="rootNode">根 node</param>
        /// <returns></returns>
        public static ChartFavoriteListTreeNode_Chart GetFavoriteChartNodeFromTree(long chartFavorId,
            ChartFavoriteListTreeNode_Folder rootNode)
        {
            if (rootNode == null) return null;
            foreach (var childNode in rootNode.Children)
            {
                if (childNode is ChartFavoriteListTreeNode_Chart chartChildNode 
                    && childNode.FavoriteId == chartFavorId)
                {
                    return chartChildNode;
                }
                else if (childNode is ChartFavoriteListTreeNode_Folder folderChildNode)
                {
                    var tarNode = GetFavoriteChartNodeFromTree(chartFavorId, folderChildNode);
                    if (tarNode != null)
                    {
                        return tarNode;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 递归删除收藏项
        /// </summary>
        /// <param name="rootNodeList">收藏项列表</param>
        /// <param name="rmNode">要删除的项</param>
        public static void RemoveFavoriteNodeRecursive(ICollection<ChartFavoriteListTreeNodeBase> rootNodeList, ChartFavoriteListTreeNodeBase rmNode)
        {
            if (rootNodeList?.Any() != true) return;
            rootNodeList.Remove(rmNode);
            foreach (var _node in rootNodeList)
            {
                if (_node is ChartFavoriteListTreeNode_Folder _folderNode)
                {
                    RemoveFavoriteNodeRecursive(_folderNode.Children, rmNode);
                }
            }
        }

        /// <summary>
        /// 递归删除收藏项
        /// </summary>
        /// <param name="rootNodeList">收藏项列表</param>
        /// <param name="rmFavorItemType">要删除的收藏项类型</param>
        /// <param name="rmFavorId">要删除的收藏项 id</param>
        public static void RemoveFavoriteNodeRecursive(ICollection<ChartFavoriteListTreeNodeBase> rootNodeList,
            ChartFolderListItemType rmFavorItemType, long rmFavorId)
        {
            if (rootNodeList?.Any() != true) return;

            ChartFavoriteListTreeNodeBase[] rmList = null;
            if (rmFavorItemType == ChartFolderListItemType.Folder)
            {
                rmList = rootNodeList.OfType<ChartFavoriteListTreeNode_Folder>().Where(i => i.FavoriteId == rmFavorId).ToArray();
            }
            else if (rmFavorItemType == ChartFolderListItemType.Chart)
            {
                rmList = rootNodeList.OfType<ChartFavoriteListTreeNode_Chart>().Where(i => i.FavoriteId == rmFavorId).ToArray();
            }
            foreach (var rmNode in rmList)
            {
                rootNodeList.Remove(rmNode);
            }

            foreach (var _node in rootNodeList)
            {
                if (_node is ChartFavoriteListTreeNode_Folder _folderNode)
                {
                    RemoveFavoriteNodeRecursive(_folderNode.Children, rmFavorItemType, rmFavorId);
                }
            }
        }

        /// <summary>
        /// 从某个列表中递归删除文件夹收藏项
        /// </summary>
        /// <param name="folderNodeList">收藏项文件夹列表</param>
        /// <param name="rmFolderFavorId">要删除的文件夹收藏项 id</param>
        public static void RemoveFavoriteFolderItemInFolderNodeList(
            ICollection<ChartFavoriteListTreeNode_Folder> folderNodeList,
            long rmFolderFavorId)
        {
            if (folderNodeList?.Any() != true) return;

            var loopNodeList = folderNodeList.ToArray();
            foreach (var folderNode in loopNodeList)
            {
                var rmFolderNode = DMConstructHelper.GetFavoriteFolderNodeFromTree(rmFolderFavorId, folderNode);
                if (rmFolderNode != null)
                {
                    if (folderNode == rmFolderNode)
                    {
                        folderNodeList.Remove(folderNode);
                    }
                    else
                    {
                        DMConstructHelper.RemoveFavoriteNodeRecursive(folderNode.Children, rmFolderNode);
                    }
                }
            }
        }
    }
}
