using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace NativeModel.Trade
{
    public class NativeComposeView : Model
    {
        public NativeComposeView(long composeGraphId, int subUserId)
        {
            this.ComposeGraphId = composeGraphId;
            this.SubUserId = subUserId;
        }

        public long ComposeGraphId { get; private set; }

        public int SubUserId { get; private set; }
        
        private string aliasName;
        public string AliasName
        {
            get { return aliasName; }
            set { SetProperty(ref aliasName, value); }
        }

        private long createTimestamp;
        public long CreateTimestamp
        {
            get { return createTimestamp; }
            set { SetProperty(ref createTimestamp, value); }
        }

        private ClientComposeViewSubscribeStatus subscribeStatus;
        public ClientComposeViewSubscribeStatus SubscribeStatus
        {
            get { return subscribeStatus; }
            set { SetProperty(ref subscribeStatus, value); }
        }

        private short precisionNumber;
        public short PrecisionNumber
        {
            get { return precisionNumber; }
            set { SetProperty(ref precisionNumber, value); }
        }
    }
}
