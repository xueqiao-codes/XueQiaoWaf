using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib.xqclient_base.thriftapi_mediation.Interface;

namespace lib.xqclient_base.thriftapi_mediation
{
    public class InterfaceCustomParsedExceptionResultHolder : IInterfaceCustomParsedExceptionResult
    {
        public int? BusinessErrorCode { get; set; }

        public string BusinessErrorMessage { get; set; }

        public bool SessionInvalid { get; set; } = false;
    }
}
