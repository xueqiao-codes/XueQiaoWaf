using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Manage.Applications.DataModels
{
    public class ManageItemModel : Model
    {
        private string itemIitle;
        public string ItemTitle
        {
            get { return itemIitle; }
            set { SetProperty(ref itemIitle, value); }
        }

        private object itemContentView;
        public object ItemContentView
        {
            get { return itemContentView; }
            set { SetProperty(ref itemContentView, value); }
        }

    }
}
