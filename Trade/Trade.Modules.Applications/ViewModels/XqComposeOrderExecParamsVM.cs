using NativeModel.Trade;
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
    public class XqComposeOrderExecParamsVM : ViewModel<IXqComposeOrderExecParamsView>
    {
        [ImportingConstructor]
        protected XqComposeOrderExecParamsVM(IXqComposeOrderExecParamsView view) : base(view)
        {
            LegExecParamsList = new ObservableCollection<XqComposeOrderLegExecParamsDM>();
        }

        private XQComposeOrderExecParamsSendType? orderSendType;
        /// <summary>
        /// 发单方式
        /// </summary>
        public XQComposeOrderExecParamsSendType? OrderSendType
        {
            get { return orderSendType; }
            set { SetProperty(ref orderSendType, value); }
        }

        private int? earlySuspendedForMarketSeconds;
        public int? EarlySuspendedForMarketSeconds
        {
            get { return earlySuspendedForMarketSeconds; }
            set { SetProperty(ref earlySuspendedForMarketSeconds, value); }
        }

        /// <summary>
        /// 各腿的执行参数
        /// </summary>
        public ObservableCollection<XqComposeOrderLegExecParamsDM> LegExecParamsList { get; private set; }

        private XqComposeOrderFirstLegChooseDM firstLegChooseDM;
        /// <summary>
        /// 先手选择信息
        /// </summary>
        public XqComposeOrderFirstLegChooseDM FirstLegChooseDM
        {
            get { return firstLegChooseDM; }
            set { SetProperty(ref firstLegChooseDM, value); }
        }
    }
}
