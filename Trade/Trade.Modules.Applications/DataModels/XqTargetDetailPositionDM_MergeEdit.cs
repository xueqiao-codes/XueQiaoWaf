using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using NativeModel.Trade;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 用于 merge 的持仓详细编辑项
    /// </summary>
    public class XqTargetDetailPositionDM_MergeEdit : XqTargetDetailPositionDM
    {
        public XqTargetDetailPositionDM_MergeEdit(string targetKey, ClientXQOrderTargetType targetType,
            long subAccountId, long detailItemId) : base(targetKey, targetType, subAccountId)
        {
            this.DetailItemId = detailItemId;
        }

        /// <summary>
        /// 明细项 id
        /// </summary>
        public long DetailItemId { get; private set; }

        private int inputQuantity;
        /// <summary>
        /// 当前输入的数量
        /// </summary>
        public int InputQuantity
        {
            get { return inputQuantity; }
            set { SetProperty(ref inputQuantity, value); }
        }

        private int maximumInputQuantity = int.MaxValue;
        /// <summary>
        /// 最大可输入数量
        /// </summary>
        public int MaximumInputQuantity
        {
            get { return maximumInputQuantity; }
            set { SetProperty(ref maximumInputQuantity, value); }
        }
    }
}
