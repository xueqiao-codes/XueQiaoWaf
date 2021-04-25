using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Navigation;

namespace Manage.Applications.Views
{
    public interface ISubAccountAddWizardDialogContentView : IView
    {
        /// <summary>
        /// 显示某个页面
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="initialWidth">页面初始宽度</param>
        /// <param name="initialHeight">页面初始高度</param>
        /// <param name="wizardReturnHandler">返回结果处理</param>
        void Navigate(PageFunction<int> page, double? initialWidth, double? initialHeight, Action<int> wizardReturnHandler);

        object DisplayInWindow { get; }

        void CloseDisplayInWindow();
    }
}
