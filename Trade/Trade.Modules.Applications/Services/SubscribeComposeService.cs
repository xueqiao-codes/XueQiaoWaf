using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    /// <summary>
    /// 订阅的组合数据服务
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class SubscribeComposeService : Model
    {
        public SubscribeComposeService()
        {
            Composes = new ObservableCollection<SubscribeComposeDataModel>();
        }

        public ObservableCollection<SubscribeComposeDataModel> Composes { get; private set; }

    }
}
