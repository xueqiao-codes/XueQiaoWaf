using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class MainWorkspaceComponentLayoutViewModel : ViewModel<IMainWorkspaceComponentLayoutView>
    {
        [ImportingConstructor]
        protected MainWorkspaceComponentLayoutViewModel(IMainWorkspaceComponentLayoutView view) : base(view)
        {
        }

        private object componentHeaderView;
        public object ComponentHeaderView
        {
            get { return componentHeaderView; }
            set { SetProperty(ref componentHeaderView, value); }
        }

        private object componentContentView;
        public object ComponentContentView
        {
            get { return componentContentView; }
            set { SetProperty(ref componentContentView, value); }
        }
    }
}
