using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers.Events
{
    internal class SubscribeDataGroupsOrderIndexChangedEventArgs
    {
        public SubscribeDataGroupsOrderIndexChangedEventArgs(object sender, IDictionary<SubscribeDataGroup, int> groupOrderIndexes)
        {
            this.Sender = sender;
            this.GroupOrderIndexes = groupOrderIndexes;
        }

        public readonly object Sender;

        /// <summary>
        /// 分组的顺序字典
        /// </summary>
        public readonly IDictionary<SubscribeDataGroup, int> GroupOrderIndexes;
    }
}
