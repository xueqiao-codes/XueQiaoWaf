using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.quotation;
using xueqiao.trade.hosting;

namespace XueQiaoFoundation.Interfaces.Event.quotation_server
{
    public class ServerConnectInfo
    {
        public string ConnectHost { get; set; }

        public int ConnectPort { get; set; }
    }

    public class ServerConnectOpenEventMsg
    {
        public bool IsOpened { get; set; }

        public ServerConnectInfo ConnectInfo { get; set; }
    }

    /// <summary>
    /// 行情服务连接开始的事件
    /// </summary>
    public class ServerConnectOpen : PubSubEvent<ServerConnectOpenEventMsg> { }

    /// <summary>
    /// 行情服务连接关闭的事件
    /// </summary>
    public class ServerConnectClose : PubSubEvent { }
    
}
