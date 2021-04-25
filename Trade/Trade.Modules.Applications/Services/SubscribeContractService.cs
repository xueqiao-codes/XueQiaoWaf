using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    /// <summary>
    /// 订阅的合约数据服务
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class SubscribeContractService 
    {
        public SubscribeContractService()
        {
            Contracts = new ObservableCollection<SubscribeContractDataModel>();
        }

        // 主线程处理，否则会出现异常
        public ObservableCollection<SubscribeContractDataModel> Contracts { get; private set; }
    }
}
