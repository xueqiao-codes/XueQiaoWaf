using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.trade.hosting;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class AppStatusBarVM : ViewModel<IAppStatusBarView>
    {
        [ImportingConstructor]
        public AppStatusBarVM(IAppStatusBarView view) : base(view)
        {

        }


        private string loginUserName;
        public string LoginUserName
        {
            get { return loginUserName; }
            set { SetProperty(ref loginUserName, value); }
        }
        
        private string apiLibEnvironmentInfo;
        public string ApiLibEnvironmentInfo
        {
            get { return apiLibEnvironmentInfo; }
            set { SetProperty(ref apiLibEnvironmentInfo, value); }
        }

        private string loginCompanyName;
        public string LoginCompanyName
        {
            get { return loginCompanyName; }
            set { SetProperty(ref loginCompanyName, value); }
        }

        private string loginCompanyGroupName;
        public string LoginCompanyGroupName
        {
            get { return loginCompanyGroupName; }
            set { SetProperty(ref loginCompanyGroupName, value); }
        }

        private HostingRunningMode hostingRunningMode;
        public HostingRunningMode HostingRunningMode
        {
            get { return hostingRunningMode; }
            set { SetProperty(ref hostingRunningMode, value); }
        }

        private string quotationPushStateMsg;
        public string QuotationPushStateMsg
        {
            get { return quotationPushStateMsg; }
            set { SetProperty(ref quotationPushStateMsg, value); }
        }

        // 总 cpu 使用率
        private string totalCpuUsage;
        public string TotalCpuUsage
        {
            get { return totalCpuUsage; }
            set { SetProperty(ref totalCpuUsage, value); }
        }

        // app 的 cpu 使用率
        private string appCpuUsage;
        public string AppCpuUsage
        {
            get { return appCpuUsage; }
            set { SetProperty(ref appCpuUsage, value); }
        }

        // app 使用的内存
        private string appMemoryUsage;
        public string AppMemoryUsage
        {
            get { return appMemoryUsage; }
            set { SetProperty(ref appMemoryUsage, value); }
        }

        // 剩余可用内存
        private string restAvailableMemory;
        public string RestAvailableMemory
        {
            get { return restAvailableMemory; }
            set { SetProperty(ref restAvailableMemory, value); }
        }

        // app 使用的线程数
        private string appThreadCount;
        public string AppThreadCount
        {
            get { return appThreadCount; }
            set { SetProperty(ref appThreadCount, value); }
        }
        
        /// <summary>
        /// 交易异常项数目（包含订单异常、状态不确定订单、瘸腿待处理项）
        /// </summary>
        private int tradeExceptionItemCount;
        public int TradeExceptionItemCount
        {
            get { return tradeExceptionItemCount; }
            set { SetProperty(ref tradeExceptionItemCount, value); }
        }

        private ICommand openOrHideExceptionOrderPanelCmd;
        public ICommand OpenOrHideExceptionOrderPanelCmd
        {
            get { return openOrHideExceptionOrderPanelCmd; }
            set { SetProperty(ref openOrHideExceptionOrderPanelCmd, value); }
        }
    }
}
