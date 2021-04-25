using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Input;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels
{
    /// <summary>
    /// 用户应用下载进度 DataContext
    /// </summary>
    public class UserAppDowloadProgressDataContext : Model
    {
        private bool isDownloading;
        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool IsDownloading
        {
            get { return isDownloading; }
            set { SetProperty(ref isDownloading, value); }
        }
        
        private int downloadProgress;
        /// <summary>
        /// 下载进度。在 0-100 之间
        /// </summary>
        public int DownloadProgress
        {
            get { return downloadProgress; }
            set { SetProperty(ref downloadProgress, value); }
        }

        private ICommand cancelDownloadCmd;
        /// <summary>
        /// 取消下载 command
        /// </summary>
        public ICommand CancelDownloadCmd
        {
            get { return cancelDownloadCmd; }
            set { SetProperty(ref cancelDownloadCmd, value); }
        }
    }
}
