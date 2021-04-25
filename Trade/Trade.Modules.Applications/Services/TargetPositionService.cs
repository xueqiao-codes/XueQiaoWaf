using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class TargetPositionService : Model
    {
        public TargetPositionService()
        {
            PositionItems = new ObservableCollection<TargetPositionDataModel>();
        }

        /// <summary>
        /// 持仓列表
        /// </summary>
        public ObservableCollection<TargetPositionDataModel> PositionItems { get; private set; }
    }
}
