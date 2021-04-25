using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace XueQiaoWaf.Trade.Modules.Applications.Views
{
    public interface IComposeSearchPopupView : IView
    {
        event EventHandler Closed;

        void Close();

        void ShowPopup(object targetElement);

        /// <summary>
        /// 滚动到组合选择列表的某一项
        /// </summary>
        /// <param name="listItemData"></param>
        void ScrollToComposeItemWithData(object composeItem);

        /// <summary>
        /// 将焦点设置到搜索框
        /// </summary>
        void FocusSearchTextBox();
    }
}
