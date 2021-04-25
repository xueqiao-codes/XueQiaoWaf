using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class MorePopupVM : ViewModel<IMorePopupView>
    {
        [ImportingConstructor]
        protected MorePopupVM(IMorePopupView view) : base(view)
        {
        }

        private ICommand showApplicationInfoCmd;
        public ICommand ShowApplicationInfoCmd
        {
            get { return showApplicationInfoCmd; }
            set { SetProperty(ref showApplicationInfoCmd, value); }
        }

        private ICommand postFeedbackCmd;
        public ICommand PostFeedbackCmd
        {
            get { return postFeedbackCmd; }
            set { SetProperty(ref postFeedbackCmd, value); }
        }

        private ICommand updatePrefSettingCmd;
        public ICommand UpdatePrefSettingCmd
        {
            get { return updatePrefSettingCmd; }
            set { SetProperty(ref updatePrefSettingCmd, value); }
        }

        private ICommand updateLoginPwdCmd;
        public ICommand UpdateLoginPwdCmd
        {
            get { return updateLoginPwdCmd; }
            set { SetProperty(ref updateLoginPwdCmd, value); }
        }

        //private ICommand changeLoginAccountCmd;
        //public ICommand ChangeLoginAccountCmd
        //{
        //    get { return changeLoginAccountCmd; }
        //    set { SetProperty(ref changeLoginAccountCmd, value); }
        //}

        private ICommand exitAppCmd;
        public ICommand ExitAppCmd
        {
            get { return exitAppCmd; }
            set { SetProperty(ref exitAppCmd, value); }
        }

        private bool hasNewAppVersion;
        public bool HasNewAppVersion
        {
            get { return hasNewAppVersion; }
            set { SetProperty(ref hasNewAppVersion, value); }
        }

        private PlacementMode popupPlacement;
        public PlacementMode PopupPlacement
        {
            get { return popupPlacement; }
            set { SetProperty(ref popupPlacement, value); }
        }

        public event EventHandler Closed
        {
            add { ViewCore.Closed += value; }
            remove { ViewCore.Closed -= value; }
        }

        public void ShowPopup(object targetElement)
        {
            ViewCore.ShowPopup(targetElement);
        }

        public void Close()
        {
            ViewCore.Close();
        }
    }
}
