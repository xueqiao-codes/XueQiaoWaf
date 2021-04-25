using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface IContractListView : IView
    {
        /// <summary>
        /// 获取某个订阅条目的显示元素
        /// </summary>
        /// <param name="subscribeItemData"></param>
        /// <returns></returns>
        UIElement SubscribeItemElement(object subscribeItemData);
        
        IEnumerable<ListColumnInfo> ListDisplayColumnInfos { get; }

        void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos);
    }
}
