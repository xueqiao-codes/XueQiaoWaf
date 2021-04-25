using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    /// <summary>
    /// 散列持仓数据
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class PositionDiscreteItemsService
    {
        private Dictionary<PositionDiscreteItemKey, PositionDiscreteItemDataModel> positionItemsMap;

        public PositionDiscreteItemsService()
        {
            PositionItems = new ObservableCollection<PositionDiscreteItemDataModel>();
        }

        /// <summary>
        /// 持仓列表
        /// </summary>
        public ObservableCollection<PositionDiscreteItemDataModel> PositionItems { get; private set; }
    }
}
