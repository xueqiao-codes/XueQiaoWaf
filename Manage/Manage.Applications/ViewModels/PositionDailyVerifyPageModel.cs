using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.trade.hosting.position.adjust.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionDailyVerifyPageModel : ViewModel<IPositionDailyVerifyPage>
    {
        [ImportingConstructor]
        protected PositionDailyVerifyPageModel(IPositionDailyVerifyPage view) : base(view)
        {
        }

        private ICommand pageGoBackCmd;
        public ICommand PageGoBackCmd
        {
            get { return pageGoBackCmd; }
            set { SetProperty(ref pageGoBackCmd, value); }
        }

        private ICommand toUpdateVerifyStatusCmd;
        public ICommand ToUpdateVerifyStatusCmd
        {
            get { return toUpdateVerifyStatusCmd; }
            set { SetProperty(ref toUpdateVerifyStatusCmd, value); }
        }

        private TargetContract_TargetContractDetail contractDetailContainer;
        public TargetContract_TargetContractDetail ContractDetailContainer
        {
            get { return contractDetailContainer; }
            set { SetProperty(ref contractDetailContainer, value); }
        }

        private DailyPositionDifference dailyDiff;
        public DailyPositionDifference DailyDiff
        {
            get { return dailyDiff; }
            set { SetProperty(ref dailyDiff, value); }
        }

        /// <summary>
        /// 交易结算单参考视图
        /// </summary>
        private object upsideSettlementReferenceView;
        public object UpsideSettlementReferenceView
        {
            get { return upsideSettlementReferenceView; }
            set { SetProperty(ref upsideSettlementReferenceView, value); }
        }

        /// <summary>
        /// 雪橇成交记录参考视图
        /// </summary>
        private object sledTradeRecordReferenceView;
        public object SledTradeRecordReferenceView
        {
            get { return sledTradeRecordReferenceView; }
            set { SetProperty(ref sledTradeRecordReferenceView, value); }
        }

        /// <summary>
        /// 雪橇成交录入区域视图
        /// </summary>
        private object sledTradeInputView;
        public object SledTradeInputView
        {
            get { return sledTradeInputView; }
            set { SetProperty(ref sledTradeInputView, value); }
        }
    }
}
