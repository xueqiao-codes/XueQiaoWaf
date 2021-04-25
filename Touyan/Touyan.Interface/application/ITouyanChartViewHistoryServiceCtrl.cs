using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.datamodel;
using Touyan.Interface.model;
using XueQiaoFoundation.Shared.Model;

namespace Touyan.Interface.application
{
    public delegate void TouyanChartViewHistoryListRefreshStateChanged(DataRefreshState refreshState);

    /// <summary>
    /// 浏览历史已清除 delegate
    /// </summary>
    public delegate void TouyanChartViewHistoryAdded(ChartViewHistoryDM[] addItem);

    /// <summary>
    /// 浏览历史已删除 delegate
    /// </summary>
    public delegate void TouyanChartViewHistoryRemoved(long[] rmHistoryChartId);

    /// <summary>
    /// 浏览历史已清除 delegate
    /// </summary>
    public delegate void TouyanChartViewHistoryListCleared();

    public interface ITouyanChartViewHistoryServiceCtrl
    {
        /// <summary>
        /// 列表刷新事件
        /// </summary>
        event TouyanChartViewHistoryListRefreshStateChanged HistoryListRefreshStateChanged;

        /// <summary>
        /// 刷新历史列表。必要情况下
        /// </summary>
        /// <returns></returns>
        Task<LocalDataRefreshResultWrapper<ChartViewHistoryDM[]>> RefreshHistoryListIfNeed();

        /// <summary>
        /// 强制刷新历史列表
        /// </summary>
        /// <returns></returns>
        Task<LocalDataRefreshResultWrapper<ChartViewHistoryDM[]>> RefreshHistoryListForce();

        /// <summary>
        /// 浏览历史项已添加的事件
        /// </summary>
        event TouyanChartViewHistoryAdded ViewHistoryAdded;

        /// <summary>
        /// 添加或修改历史项
        /// </summary>
        /// <param name="historyItems"></param>
        /// <param name="resetListWhenOccurSaveFileException">当发生文件保存异常时，重置保存的历史内容</param>
        /// <returns></returns>
        Task<ChartViewHistoryDM[]> RequestAddOrUpdateHistoryItem(ChartViewHistory[] historyItems, bool resetContentWhenOccurFileSaveException);
        
        /// <summary>
        /// 浏览历史项已删除的事件
        /// </summary>
        event TouyanChartViewHistoryRemoved ViewHistoryRemoved;

        /// <summary>
        /// 删除历史项
        /// </summary>
        /// <param name="chartIds"></param>
        /// <returns></returns>
        Task<Exception> RequestRemoveHistoryItem(params long[] chartIds);

        /// <summary>
        /// 浏览历史项已清除的事件
        /// </summary>
        event TouyanChartViewHistoryListCleared ViewHistoryListCleared;

        /// <summary>
        /// 清除所有历史
        /// </summary>
        /// <returns></returns>
        Task<Exception> RequestClearHistoryList();
    }
}
