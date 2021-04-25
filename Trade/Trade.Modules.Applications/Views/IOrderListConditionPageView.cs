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
    /// 预埋单列表视图
    /// </summary>
    public interface IOrderListConditionPageView : IView
    {
        IEnumerable<ListColumnInfo> ListDisplayColumnInfos { get; }

        void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos);

        void SelectAllOrderItems();
    }
}
