using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.datamodel;
using Touyan.Interface.service;

namespace Touyan.app.service
{
    [Export, Export(typeof(ITouyanChartViewHistoryService)), PartCreationPolicy(CreationPolicy.Shared)]
    public class TouyanChartViewHistoryService : ITouyanChartViewHistoryService
    {
        public TouyanChartViewHistoryService()
        {
            HistoryItems = new ObservableCollection<ChartViewHistoryDM>();
        }

        public ObservableCollection<ChartViewHistoryDM> HistoryItems { get; private set; }
    }
}
