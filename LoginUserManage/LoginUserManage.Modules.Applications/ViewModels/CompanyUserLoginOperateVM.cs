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
    public class CompanyUserLoginOperateVM : ViewModel<ICompanyUserLoginOperateView>
    {
        [ImportingConstructor]
        public CompanyUserLoginOperateVM(ICompanyUserLoginOperateView view) : base(view)
        {
        }

        /// <summary>
        /// 安全密码
        /// </summary>
        public SecureString SecurePassword => ViewCore.SecurePassword;

        private string companyCode;
        public string CompanyCode
        {
            get { return companyCode; }
            set { SetProperty(ref companyCode, value); }
        }
        
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }
        
        private bool isLogining;
        public bool IsLogining
        {
            get { return isLogining; }
            set { SetProperty(ref isLogining, value); }
        }

        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get { return loginCommand; }
            set { SetProperty(ref loginCommand, value); }
        }

        private bool isRememberLoginInfo;
        public bool IsRememberLoginInfo
        {
            get { return isRememberLoginInfo; }
            set { SetProperty(ref isRememberLoginInfo, value); }
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

        private ICommand forgetPwdCmd;
        public ICommand ForgetPwdCmd
        {
            get { return forgetPwdCmd; }
            set { SetProperty(ref forgetPwdCmd, value); }
        }
        
        private Thickness viewMargin;
        public Thickness ViewMargin
        {
            get { return viewMargin; }
            set { SetProperty(ref viewMargin, value); }
        }
    }
}
