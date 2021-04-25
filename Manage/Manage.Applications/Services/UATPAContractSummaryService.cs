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
    /// 未分配持仓、预分配以合约分组的概况 service
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class UATPAContractSummaryService : Model
    {
        public UATPAContractSummaryService()
        {
            ContractGroupedSummaries = new ObservableCollection<UATContractGroupedSummary>();
        }

        /// <summary>
        /// 以合约分组的未分配持仓概况列表
        /// </summary>
        public ObservableCollection<UATContractGroupedSummary> ContractGroupedSummaries { get; private set; }

        private int _UPATotalVolume;
        /// <summary>
        /// 未预分配(Unpreview assign)的总数
        /// </summary>
        public int UPATotalVolume
        {
            get { return _UPATotalVolume; }
            set { SetProperty(ref _UPATotalVolume, value); }
        }

        private int _PATotalVolume;
        /// <summary>
        /// 预分配(Preview assign)的总数
        /// </summary>
        public int PATotalVolume
        {
            get { return _PATotalVolume; }
            set { SetProperty(ref _PATotalVolume, value); }
        }
    }
}
