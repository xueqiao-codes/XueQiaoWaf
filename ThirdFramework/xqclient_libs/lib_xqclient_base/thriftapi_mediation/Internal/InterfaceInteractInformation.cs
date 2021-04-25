using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib.xqclient_base.thriftapi_mediation.Interface;

namespace lib.xqclient_base.thriftapi_mediation.Internal
{
    internal class InterfaceInteractInformation : IInterfaceInteractInformation
    {
        public double CostTimeMS { set; get; }

        public double BeginRequestTimestampMS { set; get; }

        public string ServiceAccessUrl { set; get; }
    }
}
