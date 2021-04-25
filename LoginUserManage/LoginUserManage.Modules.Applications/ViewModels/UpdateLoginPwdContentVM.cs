using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.LoginUserManage.Modules.Applications.Domains;
using XueQiaoWaf.LoginUserManage.Modules.Applications.Views;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UpdateLoginPwdContentVM : ViewModel<IUpdateLoginPwdContentView>
    {
        [ImportingConstructor]
        protected UpdateLoginPwdContentVM(IUpdateLoginPwdContentView view) : base(view)
        {
            this.UpdatePwd = new UpdatePwd();
        }

        public UpdatePwd UpdatePwd { get; private set; }

        private ICommand updatePwdCmd;
        public ICommand UpdatePwdCmd
        {
            get { return updatePwdCmd; }
            set { SetProperty(ref updatePwdCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }
    }
}
