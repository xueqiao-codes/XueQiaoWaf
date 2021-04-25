using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace ContainerShell.Applications.DataModels
{
    public class InitializeItem : Model
    {
        public InitializeItem()
        {
        }
        
        private bool canSkipWhenFailed;
        /// <summary>
        /// 当失败时是否可跳过初始化
        /// </summary>
        public bool CanSkipWhenFailed
        {
            get { return canSkipWhenFailed; }
            set { SetProperty(ref canSkipWhenFailed, value); }
        }

        /// <summary>
        /// 状态信息
        /// </summary>
        private string status;
        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        private bool isInitializing;
        /// <summary>
        /// 是否正在初始化
        /// </summary>
        public bool IsInitializing
        {
            get { return isInitializing; }
            set { SetProperty(ref isInitializing, value); }
        }

        private bool? isSuccess;
        /// <summary>
        /// 是否初始化成功
        /// </summary>
        public bool? IsSuccess
        {
            get { return isSuccess; }
            set { SetProperty(ref isSuccess, value); }
        }
    }
}
