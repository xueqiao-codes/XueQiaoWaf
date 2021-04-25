using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.contract.standard;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Interfaces.Applications;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class ContractBasicInfoDM : Model
    {
        private ICurrencyUnitsService currencyUnitsService;
        public ICurrencyUnitsService CurrencyUnitsService
        {
            get { return currencyUnitsService; }
            set { SetProperty(ref currencyUnitsService, value); }
        }

        private TargetContract_TargetContractDetail contractDetailContainer;
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        private SledCommodityConfig commodityConfig;
        public SledCommodityConfig CommodityConfig
        {
            get { return commodityConfig; }
            set { SetProperty(ref commodityConfig, value); }
        }
    }
}
