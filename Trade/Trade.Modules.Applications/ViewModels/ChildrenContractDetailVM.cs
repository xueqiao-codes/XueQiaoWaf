using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ChildrenContractDetailVM : ViewModel<IChildrenContractDetailView>
    {
        [ImportingConstructor]
        protected ChildrenContractDetailVM(IChildrenContractDetailView view) : base(view)
        {
            ChildrenContractBasicInfos = new ObservableCollection<ContractBasicInfoDM>();
        }

        /// <summary>
        /// 子合约信息列表
        /// </summary>
        public ObservableCollection<ContractBasicInfoDM> ChildrenContractBasicInfos { get; private set; }
    }
}
