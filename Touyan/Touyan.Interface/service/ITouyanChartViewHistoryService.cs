using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touyan.Interface.datamodel;

namespace Touyan.Interface.service
{
    public interface ITouyanChartViewHistoryService
    {
        ObservableCollection<ChartViewHistoryDM> HistoryItems { get; }
    }
}
