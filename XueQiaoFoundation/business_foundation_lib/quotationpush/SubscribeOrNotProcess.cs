using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_foundation_lib.quotationpush
{
    internal class SubscribeOrNotProcess
    {
        public long ReqId { get; private set; }

        public SubscribeQuotationKey TargetSubscribeKey { get; private set; }

        public int RetryNum { get; set; }

        // 上一次请求发送的时间戳(单位：毫秒)
        public long LastReqTimeStampMs { get; set; }
        
        public SubscribeOrNotProcess(long reqId, SubscribeQuotationKey targetSubscribeKey)
        {
            this.ReqId = reqId;
            this.TargetSubscribeKey = targetSubscribeKey;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("SubscribeProcess(");
            sb.Append("ReqId:").Append(ReqId)
            .Append(", TargetSubscribeKey: ").Append(TargetSubscribeKey.ToString())
            .Append(", RetryNum: ").Append(RetryNum)
            .Append(", LastReqTimeStampMs: ").Append(LastReqTimeStampMs);
            return sb.ToString();
        }
    }
}
