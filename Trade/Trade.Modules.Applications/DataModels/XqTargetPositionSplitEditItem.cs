using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 雪橇标的持仓拆分编辑项 data model
    /// </summary>
    public class XqTargetPositionSplitEditItem : XqTargetDetailPositionDM_Unarchived
    {
        public XqTargetPositionSplitEditItem(string targetKey, ClientXQOrderTargetType targetType, long subAccountId, long detailItemId) 
            : base(targetKey, targetType, subAccountId, detailItemId)
        {
        }
        
        private int splitQuantity;
        /// <summary>
        /// 拆分数量
        /// </summary>
        public int SplitQuantity
        {
            get { return splitQuantity; }
            set { SetProperty(ref splitQuantity, value); }
        }
    }
}
