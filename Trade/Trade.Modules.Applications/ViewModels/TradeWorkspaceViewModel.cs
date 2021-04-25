using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.trade.hosting;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TradeWorkspaceViewModel : ViewModel<ITradeWorkspaceView>
    {
        private readonly FundItemsService fundItemsService;

        [ImportingConstructor]
        public TradeWorkspaceViewModel(ITradeWorkspaceView view,
            FundItemsService fundItemsService) : base(view)
        {
            this.DraggableComponentPanelContext = new DraggableComponentPanelContext(view);
            this.UserRelatedSubAccountItems = new ObservableCollection<HostingSubAccountRelatedItem>();

            this.fundItemsService = fundItemsService;

            CollectionChangedEventManager.AddHandler(fundItemsService.FundItems, FundItemsCollectionChanged);
            InvalidateSubAccountBaseCurrencyFund();
        }
        
        /// <summary>
        /// 组件面板上下文
        /// </summary>
        public DraggableComponentPanelContext DraggableComponentPanelContext { get; private set; }
        
        private FundItemDataModel subAccountBaseCurrencyFund;
        /// <summary>
        /// 子账号的基币权益
        /// </summary>
        public FundItemDataModel SubAccountBaseCurrencyFund
        {
            get { return subAccountBaseCurrencyFund; }
            set { SetProperty(ref subAccountBaseCurrencyFund, value); }
        }
        
        private ReadOnlyObservableCollection<TradeTabWorkspaceTemplate> tradeWorkspaceTemplates;
        /// <summary>
        /// 交易工作区模板列表
        /// </summary>
        public ReadOnlyObservableCollection<TradeTabWorkspaceTemplate> TradeWorkspaceTemplates
        {
            get { return tradeWorkspaceTemplates; }
            set { SetProperty(ref tradeWorkspaceTemplates, value); }
        }

        private ICommand closeComponentCommand;
        public ICommand CloseComponentCommand
        {
            get { return closeComponentCommand; }
            set { SetProperty(ref closeComponentCommand, value); }
        }

        private ICommand saveCurrentWorkspaceCommand;
        public ICommand SaveCurrentWorkspaceCommand
        {
            get { return saveCurrentWorkspaceCommand; }
            set { SetProperty(ref saveCurrentWorkspaceCommand, value); }
        }

        private ICommand workspaceTemplateManageCommand;
        public ICommand WorkspaceTemplateManageCommand
        {
            get { return workspaceTemplateManageCommand; }
            set { SetProperty(ref workspaceTemplateManageCommand, value); }
        }

        private ICommand toApplyDefaultTemplateCmd;
        public ICommand ToApplyDefaultTemplateCmd
        {
            get { return toApplyDefaultTemplateCmd; }
            set { SetProperty(ref toApplyDefaultTemplateCmd, value); }
        }
        
        private ICommand openWorkspaceTemplateCommand;
        public ICommand OpenWorkspaceTemplateCommand
        {
            get { return openWorkspaceTemplateCommand; }
            set { SetProperty(ref openWorkspaceTemplateCommand, value); }
        }

        private ICommand pickComponentCommand;
        public ICommand PickComponentCommand
        {
            get { return pickComponentCommand; }
            set { SetProperty(ref pickComponentCommand, value); }
        }

        private ICommand addComponentCommand;
        public ICommand AddComponentCommand
        {
            get { return addComponentCommand; }
            set { SetProperty(ref addComponentCommand, value); }
        }

        private ICommand addChartComponentCmd;
        public ICommand AddChartComponentCmd
        {
            get { return addChartComponentCmd; }
            set { SetProperty(ref addChartComponentCmd, value); }
        }

        private ICommand addPlaceOrderComponentCmd;
        public ICommand AddPlaceOrderComponentCmd
        {
            get { return addPlaceOrderComponentCmd; }
            set { SetProperty(ref addPlaceOrderComponentCmd, value); }
        }

        private ICommand addChartAndOrderComponentCmd;
        public ICommand AddChartAndOrderComponentCmd
        {
            get { return addChartAndOrderComponentCmd; }
            set { SetProperty(ref addChartAndOrderComponentCmd, value); }
        }
        
        public ObservableCollection<HostingSubAccountRelatedItem> UserRelatedSubAccountItems { get; private set; }

        private HostingSubAccountRelatedItem selectedRelatedSubAccountItem;
        public HostingSubAccountRelatedItem SelectedRelatedSubAccountItem
        {
            get { return selectedRelatedSubAccountItem; }
            set
            {
                SetProperty(ref selectedRelatedSubAccountItem, value);
                InvalidateSubAccountBaseCurrencyFund();
            }
        }
        
        private void FundItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InvalidateSubAccountBaseCurrencyFund();
        }

        private void InvalidateSubAccountBaseCurrencyFund()
        {
            this.SubAccountBaseCurrencyFund = fundItemsService.FundItems.ToArray()
                .FirstOrDefault(i => i.IsBaseCurrency && i.SubAccountFields.SubAccountId == (this.SelectedRelatedSubAccountItem?.SubAccountId??0));
        }
    }
}
