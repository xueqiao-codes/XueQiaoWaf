using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.Trade.Modules.Domain.Properties;

namespace XueQiaoWaf.Trade.Modules.Domain.Trades
{
    public class AddCompose : ValidationModel, IValidatableObject
    {
        private string composeName;
        private string formular;
        private short precisionNumber;

        public AddCompose()
        {
            ComposeUnits = new ObservableCollection<AddComposeUnit>();
        }

        [Required(ErrorMessageResourceName ="AddComposeNameRequired",ErrorMessageResourceType =typeof(Resources))]
        public string ComposeName
        {
            get { return composeName; }
            set { SetProperty(ref composeName, value); }
        }

        [Required(ErrorMessageResourceName = "AddComposeFormularRequired", ErrorMessageResourceType = typeof(Resources))]
        public string Formular
        {
            get { return formular; }
            set { SetProperty(ref formular, value); }
        }

        [Required(ErrorMessageResourceName = "AddComposePrecisionNumberRequired", ErrorMessageResourceType = typeof(Resources))]
        [Range(0, short.MaxValue)]
        public short PrecisionNumber
        {
            get { return precisionNumber; }
            set { SetProperty(ref precisionNumber, value); }
        }

        public ObservableCollection<AddComposeUnit> ComposeUnits { get; private set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            this.ValidComposeUnitsField(validationResults);
            return validationResults;
        }

        private void ValidComposeUnitsField(ICollection<ValidationResult> validationResults)
        {
            var ComposeUnitsField = new[] { nameof(ComposeUnits) };
            int unitMinNumber = 2;
            if (ComposeUnits.Count() < unitMinNumber)
            {
                var error = String.Format(Resources.AddComposeUnitNumbersMustEqualOrLarger, unitMinNumber);
                validationResults.Add(new ValidationResult(error, ComposeUnitsField));
                return;
            }

            if (ComposeUnits.Any(i => i.IsJoinTrade && i.Quantity <= 0))
            {
                validationResults.Add(new ValidationResult(Resources.AddComposeJoinTradeUnitQuantityLargerThan0));
                return;
            }

            foreach (var unit in ComposeUnits)
            {
                var errorMessage = unit.Validate();
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    validationResults.Add(new ValidationResult(errorMessage, ComposeUnitsField));
                }
            }
        }
    }
}
