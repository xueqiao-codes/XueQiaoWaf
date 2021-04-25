using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ChooseLegPositions2MergeDialogCtrl : IController
    {
        private readonly ChooseLegPositions2MergeVM contentVM;
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly DelegateCommand closeDialogCmd;

        private IMessageWindow dialog;

        [ImportingConstructor]
        public ChooseLegPositions2MergeDialogCtrl(
            ChooseLegPositions2MergeVM contentVM,
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator)
        {
            this.contentVM = contentVM;
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            closeDialogCmd = new DelegateCommand(CloseDialog);
        }

        public object DialogOwner { get; set; }

        public CPMergeLegInfoSynchronizer MergeLegInfoSynchronizer { get; set; }

        public void Initialize()
        {
            Debug.Assert(MergeLegInfoSynchronizer != null);

            contentVM.MergeLegInfoSynchronizer = MergeLegInfoSynchronizer;
            contentVM.CloseDialogCmd = closeDialogCmd;
        }

        public void Run()
        {
            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, new Size(600, 500), true, true,
                true, "选择腿的持仓进行合并", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
        }

        private void CloseDialog()
        {
            Shutdown();
        }

        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }
    }
}
