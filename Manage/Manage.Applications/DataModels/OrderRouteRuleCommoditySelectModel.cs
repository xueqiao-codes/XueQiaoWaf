using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Manage.Applications.DataModels
{
    public class OrderRouteRuleCommoditySelectModel : Model
    {
        private NativeCommodity commodity;
        public NativeCommodity Commodity
        {
            get { return commodity; }
            set { SetProperty(ref commodity, value); }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }
    }
}
