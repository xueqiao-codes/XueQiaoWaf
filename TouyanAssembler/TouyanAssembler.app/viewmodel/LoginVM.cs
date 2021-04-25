using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using TouyanAssembler.app.view;

namespace TouyanAssembler.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class LoginVM : ViewModel<LoginView>
    {
        [ImportingConstructor]
        public LoginVM(LoginView view) : base(view)
        {
            this.GetVerifyCodeEnabledCountDownSeconds = 0;

            IsRegisterViewChangeCmd = new DelegateCommand((object obj) =>
            {
                if (obj is bool _value)
                {
                    this.IsRegisterView = _value;
                }
            });
        }

        public DelegateCommand IsRegisterViewChangeCmd { get; private set; }

        /// <summary>
        /// 是否为注册页面。否则为登录页面
        /// </summary>
        private bool isRegisterView;
        public bool IsRegisterView
        {
            get { return isRegisterView; }
            set { SetProperty(ref isRegisterView, value); }
        }
        
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

        private bool isSubmiting;
        public bool IsSubmiting
        {
            get { return isSubmiting; }
            set { SetProperty(ref isSubmiting, value); }
        }

        private ICommand reqGetVerifyCodeCmd;
        public ICommand ReqGetVerifyCodeCmd
        {
            get { return reqGetVerifyCodeCmd; }
            set { SetProperty(ref reqGetVerifyCodeCmd, value); }
        }

        private ICommand submitCmd;
        public ICommand SubmitCmd
        {
            get { return submitCmd; }
            set { SetProperty(ref submitCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }
    }
}
