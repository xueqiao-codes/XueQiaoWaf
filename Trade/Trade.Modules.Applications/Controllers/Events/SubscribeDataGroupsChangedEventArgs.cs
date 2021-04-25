using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers.Events
{
    internal class SubscribeDataGroupsChangedEventArgs
    {
        public SubscribeDataGroupsChangedEventArgs(object sender, IEnumerable<SubscribeDataGroup> oldGroups, IEnumerable<SubscribeDataGroup> newGroups)
        {
            this.Sender = sender;
            this.OldGroups = oldGroups;
            this.NewGroups = newGroups;
        }
        public object Sender { get; private set; }
        public IEnumerable<SubscribeDataGroup> OldGroups { get; private set; }
        public IEnumerable<SubscribeDataGroup> NewGroups { get; private set; }
    }
}
