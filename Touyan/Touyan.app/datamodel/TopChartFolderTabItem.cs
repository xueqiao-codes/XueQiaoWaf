using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.app.datamodel
{
    public class TopChartFolderTabItem : Model
    {
        private ChartFolder folder;
        public ChartFolder Folder
        {
            get { return folder; }
            set { SetProperty(ref folder, value); }
        }

        private object contentView;
        public object ContentView
        {
            get { return contentView; }
            set { SetProperty(ref contentView, value); }
        }
    }
}
