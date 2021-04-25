using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionOfComposeAddViewModel : ViewModel<IPositionOfComposeAddView>
    {
        [ImportingConstructor]
        protected PositionOfComposeAddViewModel(IPositionOfComposeAddView view) : base(view)
        {
            EnumHelper.GetAllTypesForEnum(typeof(ClientTradeDirection), out IEnumerable<ClientTradeDirection> allTypes);
            this.tradeDirections = allTypes;

            this.PriceInputItems = new ObservableCollection<ComposePositionPriceInputItem>();
        }

        private readonly IEnumerable<ClientTradeDirection> tradeDirections;
        public ClientTradeDirection[] TradeDirections
        {
            get
            {
                return tradeDirections.ToArray();
            }
        }

        private TargetCompose_ComposeDetail composeDetailContainer;
        /// <summary>
        /// 组合详情容器
        /// </summary>
        public TargetCompose_ComposeDetail ComposeDetailContainer
        {
            get { return composeDetailContainer; }
            set { SetProperty(ref composeDetailContainer, value); }
        }

        private UserComposeViewContainer userComposeViewContainer;
        /// <summary>
        /// 用户组合视图容器
        /// </summary>
        public UserComposeViewContainer UserComposeViewContainer
        {
            get { return userComposeViewContainer; }
            set { SetProperty(ref userComposeViewContainer, value); }
        }
        
        private ICommand triggerSelectComposeCmd;
        public ICommand TriggerSelectComposeCmd
        {
            get { return triggerSelectComposeCmd; }
            set { SetProperty(ref triggerSelectComposeCmd, value); }
        }

        private ICommand toAddCmd;
        public ICommand ToAddCmd
        {
            get { return toAddCmd; }
            set { SetProperty(ref toAddCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }

        private ICommand priceInputItemAutoCalculateCheckedCmd;
        public ICommand PriceInputItemAutoCalculateCheckedCmd
        {
            get { return priceInputItemAutoCalculateCheckedCmd; }
            set { SetProperty(ref priceInputItemAutoCalculateCheckedCmd, value); }
        }


        private ClientTradeDirection direction;
        public ClientTradeDirection Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }
        
        private int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

        public ObservableCollection<ComposePositionPriceInputItem> PriceInputItems { get; private set; }

        private ComposePositionPriceInputItem autoCalculatePriceInputItem;
        public ComposePositionPriceInputItem AutoCalculatePriceInputItem
        {
            get { return autoCalculatePriceInputItem; }
            set { SetProperty(ref autoCalculatePriceInputItem, value); }
        }

        private bool isAdding;
        /// <summary>
        /// 是否在添加中
        /// </summary>
        public bool IsAdding
        {
            get { return isAdding; }
            set { SetProperty(ref isAdding, value); }
        }
    }
}
