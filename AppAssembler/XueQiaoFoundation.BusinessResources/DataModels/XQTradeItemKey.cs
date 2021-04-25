using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 雪橇成交项 Key
    /// </summary>
    public class XQTradeItemKey : Tuple<long, long>
    {
        public XQTradeItemKey(long subAccountId, long XQTradeId) : base(subAccountId, XQTradeId)
        {
            this.SubAccountId = subAccountId;
            this.XQTradeId = XQTradeId;
        }

        public long SubAccountId { get; private set; }

        public long XQTradeId { get; private set; }

        public override string ToString()
        {
            return $"{{" +
                $"{nameof(SubAccountId)}:{SubAccountId}" +
                $"{nameof(XQTradeId)}:{XQTradeId}" +
                $"}}";
        }
    }
}
