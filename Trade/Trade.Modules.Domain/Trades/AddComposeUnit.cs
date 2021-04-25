using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.Trade.Modules.Domain.Properties;

namespace XueQiaoWaf.Trade.Modules.Domain.Trades
{
    public class AddComposeUnit : ValidationModel, IValidatableObject
    {
        private int quantity;
        private ClientTradeDirection direction;
        private int contractId;
        private bool isJoinTrade;
        
        [Range(0, int.MaxValue, ErrorMessageResourceName ="AddComposeUnitQuantityEqualOrLargerThan0", ErrorMessageResourceType =typeof(Resources))]
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
        
        public ClientTradeDirection Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }
        
        [Range(1, int.MaxValue, ErrorMessageResourceName = "AddComposeUnitContractIdLargerThan0", ErrorMessageResourceType = typeof(Resources))]
        public int ContractId
        {
            get { return contractId; }
            set { SetProperty(ref contractId, value); }
        }

        // 是否参与交易
        public bool IsJoinTrade
        {
            get { return isJoinTrade; }
            set { SetProperty(ref isJoinTrade, value); }
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            if (this.IsJoinTrade && this.Quantity <= 0)
            {
                validationResults.Add(new ValidationResult(Resources.AddComposeJoinTradeUnitQuantityLargerThan0));
            }
            return validationResults;
        }
    }
}
