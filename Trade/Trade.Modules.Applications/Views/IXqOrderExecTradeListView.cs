using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface IXqOrderExecTradeListView : IView
    {
        void UpdateListColumnsByPresentTarget(ClientXQOrderTargetType targetType);
    }
}
