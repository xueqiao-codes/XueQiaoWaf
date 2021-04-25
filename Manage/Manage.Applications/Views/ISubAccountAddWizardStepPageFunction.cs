using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Navigation;

namespace Manage.Applications.Views
{
    public interface ISubAccountAddWizardStepPageFunction
    {
        /// <summary>
        /// 到下一步页面
        /// </summary>
        void ForwardToNextStep(PageFunction<int> nextStepPage);

        /// <summary>
        /// 取消向导
        /// </summary>
        void CancelWizard();

        /// <summary>
        /// 完成向导
        /// </summary>
        void FinishedWizard();
    }
}
