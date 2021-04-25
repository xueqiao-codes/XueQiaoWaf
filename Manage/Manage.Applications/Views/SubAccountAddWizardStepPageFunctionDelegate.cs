using Manage.Applications.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Manage.Applications.Views
{
    /// <summary>
    /// 注意：不能使用该类来抽象 PageFunction 的相关业务。
    /// 因为框架对 OnReturn 的调用位置进行了限制， OnReturn 方法需要在 PageFunction 页面中直接调用。
    /// 否则会报错误 “System.NotSupportedException 类型的未经处理的异常在 PresentationFramework.dll 中发生。PageFunction 的 Return 事件处理程序需要成为父页面对象上的实例方法。” 
    /// </summary>
    public class SubAccountAddWizardStepPageFunctionDelegate : ISubAccountAddWizardStepPageFunction
    {
        public Action<object> NavigateAction;

        public Action<ReturnEventArgs<int>> OnReturnAction;
        
        public void ForwardToNextStep(PageFunction<int> nextStepPage)
        {
            nextStepPage.Return -= NextStepPage_Return;
            nextStepPage.Return += NextStepPage_Return;
            NavigateAction?.Invoke(nextStepPage);
        }

        public void CancelWizard()
        {
            // Cancel the wizard and don't return any data
            OnReturnAction?.Invoke(new ReturnEventArgs<int>(SubAccountAddWizardResultReference.Canceled));
        }

        public void FinishedWizard()
        {
            OnReturnAction?.Invoke(new ReturnEventArgs<int>(SubAccountAddWizardResultReference.Finished));
        }

        private void NextStepPage_Return(object sender, ReturnEventArgs<int> e)
        {
            // If returning, wizard was completed (finished or canceled),
            // so continue returning to calling page
            OnReturnAction?.Invoke(e);
        }
    }
}
