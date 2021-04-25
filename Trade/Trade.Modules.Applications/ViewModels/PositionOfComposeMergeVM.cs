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
    public class PositionOfComposeMergeVM : ViewModel<IPositionOfComposeMergeView>
    {
        [ImportingConstructor]
        protected PositionOfComposeMergeVM(IPositionOfComposeMergeView view) : base(view)
        {
            MergeLegInfoItems = new ObservableCollection<CPMergeLegInfoItem>();
            MergePriceInputItems = new ObservableCollection<CPMergePriceInputItem>();
        }

        public ClientTradeDirection[] TradeDirections
        {
            get
            {
                EnumHelper.GetAllTypesForEnum(typeof(ClientTradeDirection), out IEnumerable<ClientTradeDirection> allTypes);
                return allTypes?.ToArray();
            }
        }
        
        private TargetCompose_ComposeDetail selectedComposeDetailContainer;
        /// <summary>
        /// 所选组合详情容器
        /// </summary>
        public TargetCompose_ComposeDetail SelectedComposeDetailContainer
        {
            get { return selectedComposeDetailContainer; }
            set { SetProperty(ref selectedComposeDetailContainer, value); }
        }

        private UserComposeViewContainer selectedUserComposeViewContainer;
        /// <summary>
        /// 所选用户组合视图容器
        /// </summary>
        public UserComposeViewContainer SelectedUserComposeViewContainer
        {
            get { return selectedUserComposeViewContainer; }
            set { SetProperty(ref selectedUserComposeViewContainer, value); }
        }

        private ClientTradeDirection mergeTargetDirection;
        public ClientTradeDirection MergeTargetDirection
        {
            get { return mergeTargetDirection; }
            set { SetProperty(ref mergeTargetDirection, value); }
        }

        private int maxCanMergeQuantity = 0;
        public int MaxCanMergeQuantity
        {
            get { return maxCanMergeQuantity; }
            set { SetProperty(ref maxCanMergeQuantity, value); }
        }

        private int mergeTargetQuantity = 0;
        public int MergeTargetQuantity
        {
            get { return mergeTargetQuantity; }
            set { SetProperty(ref mergeTargetQuantity, value); }
        }
        
        /// <summary>
        /// 持仓合并腿信息列表
        /// </summary>
        public ObservableCollection<CPMergeLegInfoItem> MergeLegInfoItems { get; private set; }

        /// <summary>
        /// 持仓合并的价格录入项列表
        /// </summary>
        public ObservableCollection<CPMergePriceInputItem> MergePriceInputItems { get; private set; }
        
        private CPMergePriceInputItem autoCalculateMergePriceInputItem;
        /// <summary>
        /// 当前选择的价格自动计算录入项
        /// </summary>
        public CPMergePriceInputItem AutoCalculateMergePriceInputItem
        {
            get { return autoCalculateMergePriceInputItem; }
            set { SetProperty(ref autoCalculateMergePriceInputItem, value); }
        }
        
        private ICommand triggerSelectComposeCmd;
        public ICommand TriggerSelectComposeCmd
        {
            get { return triggerSelectComposeCmd; }
            set { SetProperty(ref triggerSelectComposeCmd, value); }
        }

        private ICommand toChooseLegPositionsCmd;
        public ICommand ToChooseLegPositionsCmd
        {
            get { return toChooseLegPositionsCmd; }
            set { SetProperty(ref toChooseLegPositionsCmd, value); }
        }

        private ICommand mergePriceInputItemAutoCalculateCheckedCmd;
        /// <summary>
        /// 价格录入项自动计算选择 command
        /// </summary>
        public ICommand MergePriceInputItemAutoCalculateCheckedCmd
        {
            get { return mergePriceInputItemAutoCalculateCheckedCmd; }
            set { SetProperty(ref mergePriceInputItemAutoCalculateCheckedCmd, value); }
        }
        
        private ICommand toMergeCmd;
        public ICommand ToMergeCmd
        {
            get { return toMergeCmd; }
            set { SetProperty(ref toMergeCmd, value); }
        }

        private bool isReqMerging;
        /// <summary>
        /// 是否正在合并
        /// </summary>
        public bool IsReqMerging
        {
            get { return isReqMerging; }
            set { SetProperty(ref isReqMerging, value); }
        }

        private string mergeValidateErrorText;
        public string MergeValidateErrorText
        {
            get { return mergeValidateErrorText; }
            set { SetProperty(ref mergeValidateErrorText, value); }
        }

        private bool needHideMergeOptAreaView;
        /// <summary>
        /// 合并操作区域视图是否需要隐藏
        /// </summary>
        public bool NeedHideMergeOptAreaView
        {
            get { return needHideMergeOptAreaView; }
            set { SetProperty(ref needHideMergeOptAreaView, value); }
        }
    }
}
