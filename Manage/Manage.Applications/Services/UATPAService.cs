using Manage.Applications.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Manage.Applications.Services
{
    /// <summary>
    /// 未分配成交和预分配(UnAssign Trade and Preview Assign) service
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class UATPAService : Model
    {
        public UATPAService()
        {
            UATItems = new ObservableCollection<UnAssignTradeDM>();
            PAItems = new ObservableCollection<PositionPreviewAssignDM>();
        }

        /// <summary>
        /// 未分配成交列表
        /// </summary>
        public ObservableCollection<UnAssignTradeDM> UATItems { get; private set; }

        /// <summary>
        /// 持仓预分配项列表
        /// </summary>
        public ObservableCollection<PositionPreviewAssignDM> PAItems { get; private set; }
        
    }
}
