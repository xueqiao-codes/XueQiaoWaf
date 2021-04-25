using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting;

namespace Manage.Applications.DataModels
{
    public class SubUserSelectModel : Model
    {
        public SubUserSelectModel(int subUserId)
        {
            this.SubUserId = subUserId;
        }

        public int SubUserId { get; private set; }
        
        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        private HostingUser subUserInfo;
        public HostingUser SubUserInfo
        {
            get { return subUserInfo; }
            set { SetProperty(ref subUserInfo, value); }
        }
    }
}
