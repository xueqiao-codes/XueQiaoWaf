using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.trade.hosting.proxy;
using XueQiaoWaf.LoginUserManage.Modules.Applications.Views;

namespace XueQiaoWaf.LoginUserManage.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SelectCompanyGroupContentVM : ViewModel<ISelectCompanyGroupContentView>
    {
        [ImportingConstructor]
        protected SelectCompanyGroupContentVM(ISelectCompanyGroupContentView view) : base(view)
        {
            CompanyGroups = new ObservableCollection<ProxyCompanyGroup>();
        }

        private ICommand confirmCmd;
        public ICommand ConfirmCmd
        {
            get { return confirmCmd; }
            set { SetProperty(ref confirmCmd, value); }
        }

        public ObservableCollection<ProxyCompanyGroup> CompanyGroups { get; private set; }

        private ProxyCompanyGroup selectedCompanyGroup;
        public ProxyCompanyGroup SelectedCompanyGroup
        {
            get { return selectedCompanyGroup; }
            set { SetProperty(ref selectedCompanyGroup, value); }
        }
    }
}
