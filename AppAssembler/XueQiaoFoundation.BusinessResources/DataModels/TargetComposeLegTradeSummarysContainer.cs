using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 组合标的的腿成交概要容器
    /// </summary>
    public class TargetComposeLegTradeSummarysContainer : Model
    {
        public TargetComposeLegTradeSummarysContainer(long composeGraphId)
        {
            this.ComposeGraphId = composeGraphId;
        }

        public long ComposeGraphId { get; private set; }

        private ObservableCollection<TargetComposeLegTradeSummary> legTradeSummarys;
        public ObservableCollection<TargetComposeLegTradeSummary> LegTradeSummarys
        {
            get { return legTradeSummarys; }
            set { SetProperty(ref legTradeSummarys, value); }
        }
    }
}
