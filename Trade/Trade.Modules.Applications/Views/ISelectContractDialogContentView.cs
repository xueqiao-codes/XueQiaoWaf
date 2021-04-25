using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Domain.Trades;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface ISelectContractDialogContentView : IView
    {
        object DisplayInWindow { get; }

        void CloseDisplayInWindow();
    }
}
