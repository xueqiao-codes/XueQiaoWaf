using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface IOrderHistoryParkedListView : IView
    {
        object DisplayInWindow { get; }

        IEnumerable<ListColumnInfo> ListDisplayColumnInfos { get; }

        void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos);
    }
}
