using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;

namespace lib.xqclient_base.thriftapi_mediation
{
    public class XqclientTHttpException : TException
    {
        protected int responseStatusCode;

        public XqclientTHttpException(int responseStatusCode)
            : base()
        {
            this.responseStatusCode = responseStatusCode;
        }

        public XqclientTHttpException(int responseStatusCode, string message, Exception inner = null)
            : base(message, inner)
        {
            this.responseStatusCode = responseStatusCode;
        }

        public int ResponseStatusCode
        {
            get { return responseStatusCode; }
        }
    }
}
