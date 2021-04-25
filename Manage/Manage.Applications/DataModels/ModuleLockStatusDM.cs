using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Input;
using xueqiao.trade.hosting;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 模块锁状态 data model
    /// </summary>
    public class ModuleLockStatusDM : Model
    {
        private ModuleLockState lockState;
        public ModuleLockState LockState
        {
            get { return lockState; }
            set { SetProperty(ref lockState, value); }
        }

        private HostingUser moduleLockedUser;
        /// <summary>
        /// 锁定模块的用户
        /// </summary>
        public HostingUser ModuleLockedUser
        {
            get { return moduleLockedUser; }
            set { SetProperty(ref moduleLockedUser, value); }
        }

        private ICommand requestModuleLockCmd;
        /// <summary>
        /// 请求获取模块锁并进入模块 command
        /// </summary>
        public ICommand RequestModuleLockCmd
        {
            get { return requestModuleLockCmd; }
            set { SetProperty(ref requestModuleLockCmd, value); }
        }
        
        private ICommand toExitModuleLockCmd;
        /// <summary>
        /// 退出模块锁 command
        /// </summary>
        public ICommand ToExitModuleLockCmd
        {
            get { return toExitModuleLockCmd; }
            set { SetProperty(ref toExitModuleLockCmd, value); }
        }
    }

    public enum ModuleLockState
    {
        UnLocked = 1,           // 未锁定
        LockedByOtherUser = 2,  // 被其他用户锁定
        LockedBySelf = 3,       // 被当前用户锁定
    }
}
