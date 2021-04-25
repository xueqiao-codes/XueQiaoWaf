using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    internal class XqTargetInfo
    {
        public ClientXQOrderTargetType TargetType;
        public string TargetKey;
    }

    /// <summary>
    /// 某项信息的雪橇标的解析 delegate
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    internal delegate XqTargetInfo XqTargetOfItemParser(object item);

    /// <summary>
    /// 某项信息的雪橇标的订阅行情辅助 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqTargetOfItemSubscribeQuotationHelpCtrl
    {
        private readonly ISubscribeContractController subscribeContractCtrl;
        private readonly CombQuotationSubscribeProcessCtrl combQuotationSubscribeProcessCtrl;

        private readonly DelegateCommand _subscribeTargetQuotationCmd;

        private readonly CancellationTokenSource viewCts = new CancellationTokenSource();

        [ImportingConstructor]
        public XqTargetOfItemSubscribeQuotationHelpCtrl(ISubscribeContractController subscribeContractCtrl,
            CombQuotationSubscribeProcessCtrl combQuotationSubscribeProcessCtrl)
        {
            this.subscribeContractCtrl = subscribeContractCtrl;
            this.combQuotationSubscribeProcessCtrl = combQuotationSubscribeProcessCtrl;

            _subscribeTargetQuotationCmd = new DelegateCommand(SubscribeTargetQuotation);
        }

        /// <summary>
        /// 某项信息的雪橇标的解析器
        /// </summary>
        public XqTargetOfItemParser XqTargetOfItemParser { get; set; }

        /// <summary>
        /// 信息展示窗体 owner 获取方法
        /// </summary>
        public Func<object> MsgDisplayWindowOwnerGetter { get; set; }

        /// <summary>
        /// 某项信息的标的行情订阅 command
        /// </summary>
        public ICommand SubscribeTargetQuotationCmd => _subscribeTargetQuotationCmd;

        public void Initialize()
        {
            if (XqTargetOfItemParser == null) throw new ArgumentNullException("XqTargetOfItemParser");
            if (MsgDisplayWindowOwnerGetter == null) throw new ArgumentNullException("MsgDisplayWindowOwnerGetter");
        }
        
        public void Shutdown()
        {
            if (viewCts != null)
            {
                try
                {
                    viewCts.Cancel();
                    viewCts.Dispose();
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// 订阅某项信息的标的合约
        /// </summary>
        /// <param name="item">某个需要解析的项目</param>
        public void DoSubscribeTargetQuotation(object item)
        {
            SubscribeTargetQuotation(item);
        }

        private void SubscribeTargetQuotation(object obj)
        {
            var xqTarget = XqTargetOfItemParser?.Invoke(obj);
            if (xqTarget == null) return;

            if (xqTarget.TargetType == ClientXQOrderTargetType.CONTRACT_TARGET)
            {
                var contractId = Convert.ToInt32(xqTarget.TargetKey);
                subscribeContractCtrl.AddOrUpdateSubscribeContract(contractId, SubscribeContractDataModel.SharedListContractGroupKey, null);
                subscribeContractCtrl.SubscribeContractQuotationIfNeed(contractId, null);
            }
            else if (xqTarget.TargetType == ClientXQOrderTargetType.COMPOSE_TARGET)
            {
                var composeId = Convert.ToInt64(xqTarget.TargetKey);
                if (viewCts.IsCancellationRequested) return;
                combQuotationSubscribeProcessCtrl.DoProcess(composeId, viewCts.Token,
                    () => MsgDisplayWindowOwnerGetter?.Invoke());
            }
        }
    }
}
