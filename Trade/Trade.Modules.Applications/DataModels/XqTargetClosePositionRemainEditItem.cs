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
    /// 雪橇标的配对保留编辑项 data model 
    /// </summary>
    public class XqTargetClosePositionRemainEditItem : XqTargetDetailPositionDM_Unarchived
    {
        public XqTargetClosePositionRemainEditItem(string targetKey, ClientXQOrderTargetType targetType, long subAccountId, long detailItemId) 
            : base(targetKey, targetType, subAccountId, detailItemId)
        {
        }

        private bool isEditEnabled;
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool IsEditEnabled
        {
            get { return isEditEnabled; }
            set { SetProperty(ref isEditEnabled, value); }
        }

        private int maximumRemainQuantity;
        /// <summary>
        /// 最大可保留数量
        /// </summary>
        public int MaximumRemainQuantity
        {
            get { return maximumRemainQuantity; }
            set { SetProperty(ref maximumRemainQuantity, value); }
        }
        
        private int remainQuantity;
        
        /// <summary>
        /// 保留数量
        /// </summary>
        public int RemainQuantity
        {
            get { return remainQuantity; }
            set { SetProperty(ref remainQuantity, value); }
        }
    }
}
