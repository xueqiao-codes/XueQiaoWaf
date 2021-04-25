using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace ContainerShell.Applications.Views
{
    /// <summary>
    /// 异常订单面板视图
    /// </summary>
    public interface IExceptionOrderPanelView : IView
    {
        void SelectExceptionOrderTab();
        void SelectAmbiguousOrderTab();
        void SelectTradeLameTaskNoteTab();

        void SelectExceptionOrderItemAndBringIntoView(object order);
        void SelectAmbiguousOrderItemAndBringIntoView(object order);
        void SelectTradeLameTaskNoteAndBringIntoView(object lameTaskNote);
    }
}
