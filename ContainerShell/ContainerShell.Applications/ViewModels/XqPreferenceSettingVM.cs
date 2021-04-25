using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.BusinessResources.Models;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqPreferenceSettingVM : ViewModel<IXqPreferenceSettingView>
    {
        [ImportingConstructor]
        public XqPreferenceSettingVM(IXqPreferenceSettingView view) : base(view)
        {
            AppSupportLanguageList = new ObservableCollection<XqAppLanguages>();
            AppSupportThemeList = new ObservableCollection<XqAppThemeType>();
        }

        public ObservableCollection<XqAppLanguages> AppSupportLanguageList { get; private set; }

        public ObservableCollection<XqAppThemeType> AppSupportThemeList { get; private set; }
        
        private XqAppPreferenceDM appPref;
        public XqAppPreferenceDM AppPref
        {
            get { return appPref; }
            set { SetProperty(ref appPref, value); }
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

        private ICommand selectFileSoundCmd;
        /// <summary>
        /// 参数为 <see cref="ContainerShell.Applications.DataModels.SelectFileSoundType"/>
        /// </summary>
        public ICommand SelectFileSoundCmd
        {
            get { return selectFileSoundCmd; }
            set { SetProperty(ref selectFileSoundCmd, value); }
        }
    }
}
