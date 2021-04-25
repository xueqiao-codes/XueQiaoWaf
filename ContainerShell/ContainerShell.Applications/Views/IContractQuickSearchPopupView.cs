using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace ContainerShell.Applications.Views
{
    public interface IContractQuickSearchPopupView : IView
    {
        event EventHandler Closed;

        void Close();

        void ShowPopup(object targetElement);

        /// <summary>
        /// 滚动到合约搜索结果的某一项
        /// </summary>
        /// <param name="listItemData"></param>
        void ScrollToContractSearchResultItemWithData(object listItemData);

        /// <summary>
        /// 将焦点设置到搜索框
        /// </summary>
        void FocusSearchTextBox();

        /// <summary>
        /// 获取搜索框容器元素
        /// </summary>
        object SearchBoxContainerElement { get; }
    }
}
