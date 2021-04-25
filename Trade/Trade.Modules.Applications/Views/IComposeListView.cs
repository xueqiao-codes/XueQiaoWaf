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
    public interface IComposeListView : IView
    {
        /// <summary>
        /// 获取某个订阅条目的显示元素
        /// </summary>
        /// <param name="subscribeItemData"></param>
        /// <returns></returns>
        UIElement SubscribeItemElement(object subscribeItemData);
        
        /// <summary>
        /// 组合列表当前显示的列信息
        /// </summary>
        IEnumerable<ListColumnInfo> ListDisplayColumnInfos { get; }

        /// <summary>
        /// 组合列表重置显示的列
        /// </summary>
        /// <param name="listColumnInfos"></param>
        void ResetListDisplayColumns(IEnumerable<ListColumnInfo> listColumnInfos);
    }
}
