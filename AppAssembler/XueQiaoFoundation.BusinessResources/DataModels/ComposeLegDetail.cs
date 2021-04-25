using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class ComposeLegDetail : Model
    {
        private NativeComposeLeg basicLeg;
        
        public ComposeLegDetail(NativeComposeLeg basicLeg)
        {
            this.basicLeg = basicLeg;
            this.LegDetailContainer = new TargetContract_TargetContractDetail((int)basicLeg.SledContractId);
        }

        public NativeComposeLeg BasicLeg => basicLeg;

        public TargetContract_TargetContractDetail LegDetailContainer { get; private set; }
    }
}
