using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.LoginUserManage.Modules.Applications.Views;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class Link2TouyanUserVM : ViewModel<ILink2TouyanUserView>
    {
        [ImportingConstructor]
        public Link2TouyanUserVM(ILink2TouyanUserView view) : base(view)
        {
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
