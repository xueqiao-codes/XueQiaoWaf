using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.broker;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Applications.DataModels
{
    public class BrokerAccessSelectTree
    {
        public BrokerAccessSelectTree()
        {
            Brokers = new ObservableCollection<BrokerSelectNode>();
        }

        public ObservableCollection<BrokerSelectNode> Brokers { get; private set; }
    }

    public class BrokerSelectNode : NodeWithChildren<BrokerEntry, BroderPlatformSelectNode>
    {
    }

    public class BroderPlatformSelectNode : NodeWithChildren<int, BrokerAccessEntry>
    {
    }
}
