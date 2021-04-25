using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Collections;
using Touyan.Interface.application;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.app.servicecontroller
{
    [   Export(typeof(ITouyanChartQueryCtrl)),
        PartCreationPolicy(CreationPolicy.Shared)]
    internal class TouyanChartQueryCtrl : ITouyanChartQueryCtrl
    {
        private readonly ITouyanChartCacheCtrl chartCacheCtrl;

        [ImportingConstructor]
        public TouyanChartQueryCtrl(ITouyanChartCacheCtrl chartCacheCtrl)
        {
            this.chartCacheCtrl = chartCacheCtrl;
        }
        
        public IInterfaceInteractResponse<ChartPage> RequestQueryChart(ReqChartOption queryOption, IndexedPageOption pageOption)
        {
            return InternalRequestQueryChart(queryOption, pageOption);
        }

        public Chart QueryChart(long chartId, bool ignoreCache, out IInterfaceInteractResponse _serverQueryResp)
        {
            _serverQueryResp = null;

            Chart queriedItem = null;
            if (ignoreCache == false)
            {
                queriedItem = chartCacheCtrl.GetCache(chartId);
                if (queriedItem != null)
                    return queriedItem;
            }

            var resp = InternalRequestQueryChart(new ReqChartOption { ChartIds = new THashSet<long> { chartId } }, 
                new IndexedPageOption { PageIndex = 0, PageSize = 1 });
            queriedItem = resp?.CorrectResult?.Page?.FirstOrDefault(i => i.ChartId == chartId);

            _serverQueryResp = resp;
            return queriedItem;
        }

        private IInterfaceInteractResponse<ChartPage> InternalRequestQueryChart(ReqChartOption queryOption, IndexedPageOption pageOption)
        {
            Debug.Assert(queryOption != null);
            Debug.Assert(pageOption != null);

            var resp = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                    .reqChart(queryOption, pageOption);
            var resultList = resp?.CorrectResult?.Page;
            if (resultList != null)
            {
                CacheChart(resultList);
            }
            return resp;
        }

        private void CacheChart(IEnumerable<Chart> charts)
        {
            if (charts?.Any() != true) return;
            foreach (var c in charts)
            {
                chartCacheCtrl.Cache(c.ChartId, c);
            }
        }
    }
}
