using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.datamodel;
using xueqiao.personal.user.thriftapi;
using XueQiaoFoundation.Shared.Model;

namespace Touyan.Interface.application
{
    public delegate void TouyanChartFavoriteListRefreshStateChanged(DataRefreshState refreshState);

    public delegate void TouyanChartFavoriteItemAdded(ChartFavoriteListItem addItem);

    public delegate void TouyanChartFavoriteItemRemoved(ChartFavoriteListItem rmItem);

    public delegate void TouyanChartFavoriteItemMoved(ChartFavoriteListItem movedItem, long oldParentFolderId);
    
    /// <summary>
    /// 投研图表收藏服务管理协议
    /// </summary>
    public interface ITouyanChartFavoriteServiceCtrl
    {
        /// <summary>
        /// 列表刷新事件
        /// </summary>
        event TouyanChartFavoriteListRefreshStateChanged FavoriteListRefreshStateChanged;

        /// <summary>
        /// 刷新列表。必要情况下
        /// </summary>
        /// <returns></returns>
        Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFavoriteListItem>>> RefreshFavoriteListIfNeed();

        /// <summary>
        /// 强制刷新列表
        /// </summary>
        /// <returns></returns>
        Task<ServerDataRefreshResultWrapper<IEnumerable<ChartFavoriteListItem>>> RefreshFavoriteListForce();

        /// <summary>
        /// 请求创建新的收藏文件夹
        /// </summary>
        /// <param name="favoriteInfo"></param>
        /// <returns></returns>
        Task<RequestAddFavoriteFolderResp> RequestAddFavoriteFolder(FavoriteFolder favoriteInfo);

        /// <summary>
        /// 收藏项已添加的事件
        /// </summary>
        event TouyanChartFavoriteItemAdded TouyanChartFavoriteItemAdded;

        /// <summary>
        /// 请求创建新的收藏投研图
        /// </summary>
        /// <param name="favoriteInfo"></param>
        /// <returns></returns>
        Task<RequestAddFavoriteChartResp> RequestAddFavoriteChart(FavoriteChart favoriteInfo);

        /// <summary>
        /// 收藏项已删除的事件
        /// </summary>
        event TouyanChartFavoriteItemRemoved TouyanChartFavoriteItemRemoved;

        /// <summary>
        /// 请求删除收藏文件夹
        /// </summary>
        /// <param name="folderFavorId"></param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse> RequestRemoveFavoriteFolder(long folderFavorId);

        /// <summary>
        /// 请求删除收藏投研图
        /// </summary>
        /// <param name="chartFavorId"></param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse> RequestRemoveFavoriteChart(long chartFavorId);

        /// <summary>
        /// 收藏项被移动的事件
        /// </summary>
        event TouyanChartFavoriteItemMoved TouyanChartFavoriteItemMoved;

        /// <summary>
        /// 请求移动收藏文件夹
        /// </summary>
        /// <param name="folderFavorId"></param>
        /// <param name="targetParentFolderId"></param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse> RequestMoveFavoriteFolder(long folderFavorId, long targetParentFolderId);

        /// <summary>
        /// 请求移动收藏图
        /// </summary>
        /// <param name="chartFavorId"></param>
        /// <param name="targetParentFolderId"></param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse> RequestMoveFavoriteChart(long chartFavorId, long targetParentFolderId);

        /// <summary>
        /// 请求收藏文件夹重命名
        /// </summary>
        /// <param name="folderFavorId"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        Task<IInterfaceInteractResponse> RequestRenameFavoriteFolder(long folderFavorId, string newName);
    }

    public class RequestAddFavoriteFolderResp
    {
        public IInterfaceInteractResponse Resp;

        public ChartFavoriteListItem_Folder FavoriteItem;

        public bool IsNewAdded;
    }

    public class RequestAddFavoriteChartResp
    {
        public IInterfaceInteractResponse Resp;

        public ChartFavoriteListItem_Chart FavoriteItem;

        public bool IsNewAdded;
    }
}
