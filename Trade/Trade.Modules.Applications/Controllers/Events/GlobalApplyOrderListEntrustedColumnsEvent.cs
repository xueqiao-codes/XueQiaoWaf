﻿using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers.Events
{
    /// <summary>
    /// 全局应用委托单列表列显示事件
    /// </summary>
    internal class GlobalApplyOrderListEntrustedColumnsEvent : PubSubEvent<IEnumerable<ListColumnInfo>>
    {
    }
}
