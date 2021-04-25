using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.ServiceControllers;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ComposeSearchPopupController : IController
    {
        private readonly ComposeSearchPopupViewModel popupViewModel;
        private readonly ISubscribeComposeController subscribeComposeCtrl;

        private readonly DelegateCommand selectPrevComposeCmd;
        private readonly DelegateCommand selectNextComposeCmd;
        private readonly DelegateCommand confirmSelComposeCmd;

        [ImportingConstructor]
        public ComposeSearchPopupController(ComposeSearchPopupViewModel popupViewModel,
            ISubscribeComposeController subscribeComposeCtrl)
        {
            this.popupViewModel = popupViewModel;
            this.subscribeComposeCtrl = subscribeComposeCtrl;

            selectPrevComposeCmd = new DelegateCommand(SelectPrevCompose);
            selectNextComposeCmd = new DelegateCommand(SelectNextCompose);
            confirmSelComposeCmd = new DelegateCommand(ConfirmSelCompose);
        }

        /// <summary>
        /// 弹层位置目标
        /// </summary>
        public object PopupPalcementTarget { get; set; }

        /// <summary>
        /// 关闭回调。arg0:controller, arg1:选择的组合id
        /// </summary>
        public Action<ComposeSearchPopupController, long?> PopupCloseHandler { get; set; }

        public void Initialize()
        {
            popupViewModel.Closed += PopupViewModel_Closed;
            popupViewModel.ConfirmSelComposeCmd = confirmSelComposeCmd;
            popupViewModel.SelectPrevComposeCmd = selectPrevComposeCmd;
            popupViewModel.SelectNextComposeCmd = selectNextComposeCmd;
            PropertyChangedEventManager.AddHandler(popupViewModel, PopupViewModelPropChanged, "");
        }

        public void Run()
        {
            popupViewModel.ComposeItems.AddRange(GetDataSourceComposeItems());
            popupViewModel.ShowPopup(PopupPalcementTarget);
        }

        public void Shutdown()
        {
            popupViewModel.Closed -= PopupViewModel_Closed;
            PropertyChangedEventManager.RemoveHandler(popupViewModel, PopupViewModelPropChanged, "");
        }

        private void PopupViewModel_Closed(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void PopupViewModelPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ComposeSearchPopupViewModel.SearchText))
            {
                FilterComposesWithSearchText(popupViewModel.SearchText);
                return;
            }
        }

        private void SelectPrevCompose()
        {
            SelectComposeWithOffset2Current(-1);
        }

        private void SelectNextCompose()
        {
            SelectComposeWithOffset2Current(1);
        }
        
        private void ConfirmSelCompose()
        {
            var selectedComposeId = popupViewModel.SelectedComposeItem?.ComposeId;
            if (selectedComposeId == null) return;

            popupViewModel.Closed -= PopupViewModel_Closed;
            popupViewModel.Close();

            this.PopupCloseHandler?.Invoke(this, selectedComposeId);
        }

        private void SelectComposeWithOffset2Current(int offset)
        {
            if (offset == 0) return;

            var currentItems = popupViewModel.ComposeItems;
            var originSelCompose = popupViewModel.SelectedComposeItem;
            SubscribeComposeDataModel newSelItem = null;
            if (originSelCompose == null)
            {
                newSelItem = currentItems.FirstOrDefault();
            }
            else
            {
                var originSelItemIdx = currentItems.IndexOf(originSelCompose);
                if (originSelItemIdx < 0)
                    newSelItem = currentItems.FirstOrDefault();
                else
                    newSelItem = currentItems.ElementAtOrDefault(originSelItemIdx + offset);
            }

            popupViewModel.SelectedComposeItem = newSelItem;
            if (newSelItem != null)
            {
                popupViewModel.ScrollToComposeItemWithData(newSelItem);
            }
        }

        private IEnumerable<SubscribeComposeDataModel> GetDataSourceComposeItems()
        {
            return subscribeComposeCtrl.GetSharedGroupKeySubscribeComposes();
        }

        private void FilterComposesWithSearchText(string searchText)
        {
            searchText = searchText?.Trim();

            var filerItems = GetDataSourceComposeItems();
            if (filerItems == null) filerItems = new SubscribeComposeDataModel[] { };

            popupViewModel.ComposeItems.Clear();

            if (!string.IsNullOrEmpty(searchText))
            {
                var composeTupples = filerItems.Select(i => new Tuple<SubscribeComposeDataModel, string>(i, i.UserComposeViewContainer?.UserComposeView?.AliasName))
                    .Where(i=>i.Item2?.Contains(searchText)??false)
                    .ToArray();
                filerItems = composeTupples.OrderBy(i => i.Item2.IndexOf(searchText))
                    .ToArray().Select(i=>i.Item1).ToArray();
            }

            popupViewModel.ComposeItems.AddRange(filerItems);
        }
    }
}
