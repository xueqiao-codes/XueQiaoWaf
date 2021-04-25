using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContractInfoContentVM : ViewModel<IContractInfoContentView>
    {
        [ImportingConstructor]
        protected ContractInfoContentVM(IContractInfoContentView view) : base(view)
        {
            TradeTimeSpanDailyDetails = new ObservableCollection<ContractTradeTimeSpanDailyDetail>();
        }

        /// <summary>
        /// 基本信息视图
        /// </summary>
        private object contractBasicInfoView;
        public object ContractBasicInfoView
        {
            get { return contractBasicInfoView; }
            set { SetProperty(ref contractBasicInfoView, value); }
        }

        /// <summary>
        /// 合约基本信息 data model
        /// </summary>
        private ContractBasicInfoDM contractBasicInfo;
        public ContractBasicInfoDM ContractBasicInfo
        {
            get { return contractBasicInfo; }
            set { SetProperty(ref contractBasicInfo, value); }
        }
        
        /// <summary>
        /// 合约信息
        /// </summary>
        private TargetContract_TargetContractDetail contractDetailContainer;
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        /// <summary>
        /// 触发显示子合约信息的 command
        /// </summary>
        private ICommand triggerShowChildrenContractInfoCmd;
        public ICommand TriggerShowChildrenContractInfoCmd
        {
            get { return triggerShowChildrenContractInfoCmd; }
            set { SetProperty(ref triggerShowChildrenContractInfoCmd, value); }
        }

        public ObservableCollection<ContractTradeTimeSpanDailyDetail> TradeTimeSpanDailyDetails { get; private set; }
    }
}
