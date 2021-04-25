using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;

namespace Manage.Applications.Views
{
    /// <summary>
    /// 新建操作账户向导-完成页面
    /// </summary>
    public interface ISubAccountAddWizardStepPage_Finished : IView
    {
        /// <summary>
        /// 完成向导
        /// </summary>
        void FinishedWizard();
    }
}
