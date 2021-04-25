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
    public class TradeModuleRootVM : ViewModel<ITradeModuleRootView>
    {
        [ImportingConstructor]
        public TradeModuleRootVM(ITradeModuleRootView view) : base(view)
        {
        }

        private object workspaceTabControlView;
        public object WorkspaceTabControlView
        {
            get { return workspaceTabControlView; }
            set { SetProperty(ref workspaceTabControlView, value); }
        }
    }
}
