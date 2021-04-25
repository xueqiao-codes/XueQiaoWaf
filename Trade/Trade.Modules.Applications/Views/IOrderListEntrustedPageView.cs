using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    /// <summary>
    /// 委托单列表视图
    /// </summary>
    public interface IOrderListEntrustedPageView : IView, IPageAvailableOrders
    {
        object DisplayInWindow { get; }

        IEnumerable<ListColumnInfo> ListDisplayColumnInfos { get; }

        void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos);

    }
}
