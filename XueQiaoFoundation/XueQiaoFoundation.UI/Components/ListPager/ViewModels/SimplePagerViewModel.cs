using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.UI.Components.ListPager.Views;

namespace XueQiaoFoundation.UI.Components.ListPager.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SimplePagerViewModel : ViewModel<ISimplePagerView>
    {
        [ImportingConstructor]
        public SimplePagerViewModel(ISimplePagerView view) : base(view)
        {

        }

        private PagingController pagingController;
        public PagingController PagingController
        {
            get { return pagingController; }
            set { SetProperty(ref pagingController, value); }
        }
        
        private int jumpToPage;
        public int JumpToPage
        {
            get { return jumpToPage; }
            set { SetProperty(ref jumpToPage, value); }
        }

        private ICommand goJumpPageCmd;
        public ICommand GoJumpPageCmd
        {
            get { return goJumpPageCmd; }
            set { SetProperty(ref goJumpPageCmd, value); }
        }
    }
}
