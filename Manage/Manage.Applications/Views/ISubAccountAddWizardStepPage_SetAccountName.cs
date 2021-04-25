using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Navigation;

namespace Manage.Applications.Views
{
    /// <summary>
    /// 新建操作账户向导-设置操作账户名页面
    /// </summary>
    public interface ISubAccountAddWizardStepPage_SetAccountName : IView
    {
        /// <summary>
        /// 到下一步页面
        /// </summary>
        void ForwardToNextStep(PageFunction<int> nextStepPage);
        
    }
}
