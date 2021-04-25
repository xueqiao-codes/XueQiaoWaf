using Manage.Applications.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ManageModuleRootVM : ViewModel<IManageModuleRootView>
    {
        [ImportingConstructor]
        protected ManageModuleRootVM(IManageModuleRootView view) : base(view)
        {
            WorkspaceItems = new ObservableCollection<SimpleTabItem>();
        }

        public ObservableCollection<SimpleTabItem> WorkspaceItems { get; }

        private SimpleTabItem activeWorkspaceItem;
        public SimpleTabItem ActiveWorkspaceItem
        {
            get { return activeWorkspaceItem; }
            set { SetProperty(ref activeWorkspaceItem, value); }
        }

        private ChromeWindowCaptionDataHolder embedInWindowCaptionDataHolder;
        public ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder
        {
            get { return embedInWindowCaptionDataHolder; }
            set { SetProperty(ref embedInWindowCaptionDataHolder, value); }
        }
    }
}
