using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 交易数据的操作账户数据字段
    /// </summary>
    public class SubAccountFieldsForTradeData : Model
    {
        public SubAccountFieldsForTradeData(int? subUserId, long subAccountId)
        {
            this.SubUserId = subUserId;
            this.SubAccountId = subAccountId;
        }

        /// <summary>
        /// 用户 id
        /// </summary>
        public int? SubUserId { get; private set; }

        /// <summary>
        /// 操作账户 id
        /// </summary>
        public long SubAccountId { get; private set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        private string subUserName;
        public string SubUserName
        {
            get { return subUserName; }
            set { SetProperty(ref subUserName, value); }
        }

        /// <summary>
        /// 操作账户名称
        /// </summary>
        private string subAccountName;
        public string SubAccountName
        {
            get { return subAccountName; }
            set { SetProperty(ref subAccountName, value); }
        }
    }
}
