using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Domain.Trades;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class AddComposeLegDetail : AddComposeUnit
    {
        private TargetContract_TargetContractDetail legDetailContainer;
        public TargetContract_TargetContractDetail LegDetailContainer
        {
            get { return legDetailContainer; }
            set { SetProperty(ref legDetailContainer, value); }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext);
        }
    }
}
