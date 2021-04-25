using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class FundPageController : IController
    {
        private readonly FundPageViewModel pageViewModel;

        [ImportingConstructor]
        public FundPageController(FundPageViewModel pageViewModel)
        {
            this.pageViewModel = pageViewModel;
        }

        public XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace ParentWorkspace { get; set; }

        public object ComponentContentView => pageViewModel.View;

        public void Initialize()
        {
            if (ParentWorkspace == null) throw new ArgumentNullException("ParentWorkspace");

            pageViewModel.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
            PropertyChangedEventManager.AddHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        public void Run()
        {
        }

        public void Shutdown()
        {
            PropertyChangedEventManager.RemoveHandler(ParentWorkspace, ParentWorkspacePropertyChanged, "");
        }

        private void ParentWorkspacePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(XueQiaoFoundation.BusinessResources.DataModels.TabWorkspace.SubAccountId))
            {
                pageViewModel.UpdatePresentSubAccountId(ParentWorkspace.SubAccountId);
            }
        }

    }
}
