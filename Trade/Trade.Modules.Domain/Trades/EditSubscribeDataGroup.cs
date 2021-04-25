using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoWaf.Trade.Modules.Domain.Properties;

namespace XueQiaoWaf.Trade.Modules.Domain.Trades
{
    public class EditSubscribeDataGroup : ValidatableModel
    {
        private string groupName;
        [Required(ErrorMessageResourceName = "SubscribeDataGroupNameRequired", ErrorMessageResourceType = typeof(Resources))]
        public string GroupName
        {
            get { return groupName; }
            set { SetPropertyAndValidate(ref groupName, value); }
        }
    }
}
