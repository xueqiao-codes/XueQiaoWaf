using Manage.Applications.DataModels;
using Manage.Applications.Services;
using Manage.Applications.Views;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.Shared.Helper;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionVerifyTradeInputAreaVM : ViewModel<IPositionVerifyTradeInputAreaView>
    {
        private PositionVerifyTradeInputItemsService inputItemsService;

        private long tradeAccountId;
        private long verifyDailySec;
        private int contractId;

        [ImportingConstructor]
        protected PositionVerifyTradeInputAreaVM(IPositionVerifyTradeInputAreaView view,
            PositionVerifyTradeInputItemsService inputItemsService) : base(view)
        {
            EnumHelper.GetAllTypesForEnum(typeof(ClientTradeDirection), out IEnumerable<ClientTradeDirection> allDirs);
            this.tradeDirections = allDirs?.ToArray();

            this.inputItemsService = inputItemsService;
            var syncInputItems = new SynchronizingCollection<PositionVerifyTradeInputDM, PositionVerifyTradeInputDM>(inputItemsService.XqPreviewTradeItems, i => i);
            InputItemsCollectionView = CollectionViewSource.GetDefaultView(syncInputItems) as ListCollectionView;
            ConfigInputItemsCollectionView();
            RefreshInputItemsCollectionView();
        }

        /// <summary>
        /// 更新列表的过滤条件
        /// </summary>
        /// <param name="tradeAccountId"></param>
        /// <param name="verifyDailySec"></param>
        /// <param name="contractId"></param>
        public void UpdateInputItemsFilter(long tradeAccountId, long verifyDailySec, int contractId)
        {
            this.tradeAccountId = tradeAccountId;
            this.verifyDailySec = verifyDailySec;
            this.contractId = contractId;
            RefreshInputItemsCollectionView();
        }

        /// <summary>
        /// 输入列表视图
        /// </summary>
        public ListCollectionView InputItemsCollectionView { get; private set; }
        
        /// <summary>
        /// 是否显示`在另一窗口打开`的按钮
        /// </summary>
        private bool showOpenInAnotherWindowButton;
        public bool ShowOpenInAnotherWindowButton
        {
            get { return showOpenInAnotherWindowButton; }
            set { SetProperty(ref showOpenInAnotherWindowButton, value); }
        }

        private ICommand toOpenInAnotherWindowCmd;
        public ICommand ToOpenInAnotherWindowCmd
        {
            get { return toOpenInAnotherWindowCmd; }
            set { SetProperty(ref toOpenInAnotherWindowCmd, value); }
        }
        
        private ICommand newItemCmd;
        public ICommand NewItemCmd
        {
            get { return newItemCmd; }
            set { SetProperty(ref newItemCmd, value); }
        }

        private ICommand submitAllCmd;
        public ICommand SubmitAllCmd
        {
            get { return submitAllCmd; }
            set { SetProperty(ref submitAllCmd, value); }
        }

        private ICommand submitItemCmd;
        public ICommand SubmitItemCmd
        {
            get { return submitItemCmd; }
            set { SetProperty(ref submitItemCmd, value); }
        }

        private ICommand removeItemCmd;
        public ICommand RemoveItemCmd
        {
            get { return removeItemCmd; }
            set { SetProperty(ref removeItemCmd, value); }
        }
        
        private readonly ClientTradeDirection[] tradeDirections;
        public ClientTradeDirection[] TradeDirections
        {
            get
            {
                // return new array
                return tradeDirections?.ToArray();
            }
        }

        private void ConfigInputItemsCollectionView()
        {
            var listView = InputItemsCollectionView;
            if (listView == null) return;

            listView.Filter = i =>
            {
                var item = i as PositionVerifyTradeInputDM;
                if (item == null) return false;
                return item.FundAccountId == this.tradeAccountId
                    && item.BelongVerifyDailySec == this.verifyDailySec
                    && item.ContractId == this.contractId;
            };
            listView.LiveFilteringProperties.Add(nameof(PositionVerifyTradeInputDM.FundAccountId));
            listView.LiveFilteringProperties.Add(nameof(PositionVerifyTradeInputDM.BelongVerifyDailySec));
            listView.LiveFilteringProperties.Add(nameof(PositionVerifyTradeInputDM.ContractId));
            listView.IsLiveFiltering = true;
        }

        private void RefreshInputItemsCollectionView()
        {
            InputItemsCollectionView?.Refresh();
        }
    }
}
