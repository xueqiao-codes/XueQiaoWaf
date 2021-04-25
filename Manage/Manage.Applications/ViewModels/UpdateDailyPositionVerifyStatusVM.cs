using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.trade.hosting.position.adjust.thriftapi;
using XueQiaoFoundation.Shared.Helper;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UpdateDailyPositionVerifyStatusVM : ViewModel<IUpdateDailyPositionVerifyStatusView>
    {
        [ImportingConstructor]
        protected UpdateDailyPositionVerifyStatusVM(IUpdateDailyPositionVerifyStatusView view) : base(view)
        {
            EnumHelper.GetAllTypesForEnum(typeof(VerifyStatus), out IEnumerable<VerifyStatus> allTypes);
            this.verifyStatusList = allTypes?.ToArray();
        }

        private VerifyStatus[] verifyStatusList;
        public VerifyStatus[] VerifyStatusList
        {
            get { return verifyStatusList?.ToArray(); }
        }

        private VerifyStatus? selectedVerifyStatus;
        public VerifyStatus? SelectedVerifyStatus
        {
            get { return selectedVerifyStatus; }
            set { SetProperty(ref selectedVerifyStatus, value); }
        }

        private string verifyNote;
        public string VerifyNote
        {
            get { return verifyNote; }
            set { SetProperty(ref verifyNote, value); }
        }

        private ICommand saveCmd;
        public ICommand SaveCmd
        {
            get { return saveCmd; }
            set { SetProperty(ref saveCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }
    }
}
