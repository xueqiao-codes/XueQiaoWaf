using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoWaf.Trade.Modules.Domain.Properties;

namespace XueQiaoWaf.Trade.Modules.Domain.Workspaces
{
    public class EditTradeWorkspaceTemplate : ValidatableModel
    {
        private string name;
        [Required(ErrorMessageResourceName = "TradeWorkspaceTemplateNameRequired", ErrorMessageResourceType = typeof(Resources))]
        public string Name
        {
            get { return name; }
            set { SetPropertyAndValidate(ref name, value); }
        }

        private string remark;
        public string Remark
        {
            get { return remark; }
            set { SetPropertyAndValidate(ref remark, value); }
        }
    }
}
