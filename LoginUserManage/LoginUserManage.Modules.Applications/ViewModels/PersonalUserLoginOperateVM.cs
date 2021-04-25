using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Input;
using XueQiaoWaf.LoginUserManage.Modules.Applications.Views;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PersonalUserLoginOperateVM : ViewModel<IPersonalUserLoginOperateView>
    {
        [ImportingConstructor]
        public PersonalUserLoginOperateVM(IPersonalUserLoginOperateView view) : base(view)
        {
            IsLoginByVerifyCodeChangedCmd = new DelegateCommand(obj => 
            {
                if (obj is bool changeValue)
                {
                    this.IsLoginByVerifyCode = changeValue;
                }
            });
        }

        public SecureString SecurePassword => ViewCore.SecurePassword;

        public DelegateCommand IsLoginByVerifyCodeChangedCmd { get; private set; }

        private string telNumber;
        public string TelNumber
        {
            get { return telNumber; }
            set { SetProperty(ref telNumber, value); }
        }

        private string verifyCode;
        public string VerifyCode
        {
            get { return verifyCode; }
            set { SetProperty(ref verifyCode, value); }
        }

        private bool isLoginByVerifyCode;
        public bool IsLoginByVerifyCode
        {
            get { return isLoginByVerifyCode; }
            set { SetProperty(ref isLoginByVerifyCode, value); }
        }
        
        private bool isSelectRealHostingMode;
        /// <summary>
        /// 是否选中实盘环境
        /// </summary>
        public bool IsSelectRealHostingMode
        {
            get { return isSelectRealHostingMode; }
            set { SetProperty(ref isSelectRealHostingMode, value); }
        }

        private bool isSelectSimulatorHostingMode;
        /// <summary>
        /// 是否选中模拟盘环境
        /// </summary>
        public bool IsSelectSimulatorHostingMode
        {
            get { return isSelectSimulatorHostingMode; }
            set { SetProperty(ref isSelectSimulatorHostingMode, value); }
        }
        
        private int getVerifyCodeEnabledCountDownSeconds;
        public int GetVerifyCodeEnabledCountDownSeconds
        {
            get { return getVerifyCodeEnabledCountDownSeconds; }
            set { SetProperty(ref getVerifyCodeEnabledCountDownSeconds, value); }
        }

        private bool isCountingDownGetVerifyCodeEnabled;
        public bool IsCountingDownGetVerifyCodeEnabled
        {
            get { return isCountingDownGetVerifyCodeEnabled; }
            set { SetProperty(ref isCountingDownGetVerifyCodeEnabled, value); }
        }
        
        private bool isLogining;
        public bool IsLogining
        {
            get { return isLogining; }
            set { SetProperty(ref isLogining, value); }
        }

        private ICommand reqGetVerifyCodeCmd;
        public ICommand ReqGetVerifyCodeCmd
        {
            get { return reqGetVerifyCodeCmd; }
            set { SetProperty(ref reqGetVerifyCodeCmd, value); }
        }
        
        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get { return loginCommand; }
            set { SetProperty(ref loginCommand, value); }
        }

        private ICommand registerCmd;
        public ICommand RegisterCmd
        {
            get { return registerCmd; }
            set { SetProperty(ref registerCmd, value); }
        }

        private Thickness viewMargin;
        public Thickness ViewMargin
        {
            get { return viewMargin; }
            set { SetProperty(ref viewMargin, value); }
        }

    }
}
