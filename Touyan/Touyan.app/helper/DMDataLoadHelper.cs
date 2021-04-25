using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.application;
using Touyan.Interface.datamodel;

namespace Touyan.app.helper
{
    public static class DMDataLoadHelper
    {
        /// <summary>
        /// 加载 <see cref="ChartFavoriteListItem_Chart"/> 类型数据的 ChartInfo 
        /// </summary>
        /// <param name="favorListItem_Chart"></param>
        /// <param name="chartQueryCtrl"></param>
        /// <param name="ignoreCache"></param>
        public static void LoadChartInfo(this ChartFavoriteListItem_Chart favorListItem_Chart,
            ITouyanChartQueryCtrl chartQueryCtrl, bool ignoreCache)
        {
            if (favorListItem_Chart == null) return;
            Task.Run(() => 
            {
                var chartInfo = chartQueryCtrl.QueryChart(favorListItem_Chart.ChartId, ignoreCache, out IInterfaceInteractResponse _resp);
                favorListItem_Chart.ChartInfo = chartInfo;
            });
        }

        /// <summary>
        /// 加载 <see cref="ChartViewHistoryDM"/> 类型数据的 ChartInfo 
        /// </summary>
        /// <param name="historyItem"></param>
        /// <param name="chartQueryCtrl"></param>
        /// <param name="ignoreCache"></param>
        public static void LoadChartInfo(this ChartViewHistoryDM historyItem,
            ITouyanChartQueryCtrl chartQueryCtrl, bool ignoreCache)
        {
            if (historyItem == null) return;
            Task.Run(() =>
            {
                var chartInfo = chartQueryCtrl.QueryChart(historyItem.ChartId, ignoreCache, out IInterfaceInteractResponse _resp);
                historyItem.ChartInfo = chartInfo;
            });
        }
    }
}
