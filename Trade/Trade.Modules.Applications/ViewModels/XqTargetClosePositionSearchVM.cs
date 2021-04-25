using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class XqTargetClosePositionSearchVM : ViewModel<IXqTargetClosePositionSearchView>
    {
        [ImportingConstructor]
        protected XqTargetClosePositionSearchVM(IXqTargetClosePositionSearchView view) : base(view)
        {
            EnumHelper.GetAllTypesForEnum(typeof(ClientXQOrderTargetType), out IEnumerable<ClientXQOrderTargetType> _xqTargetTypes);
            this.XqTargetTypes = _xqTargetTypes.ToArray();
            this.SelectedXqTargetType = _xqTargetTypes?.FirstOrDefault() ?? ClientXQOrderTargetType.CONTRACT_TARGET;
        }

        public ClientXQOrderTargetType[] XqTargetTypes { get; private set; }

        private ClientXQOrderTargetType selectedXqTargetType;
        public ClientXQOrderTargetType SelectedXqTargetType
        {
            get { return selectedXqTargetType; }
            set { SetProperty(ref selectedXqTargetType, value); }
        }

        private bool hasSelectedXqTarget;
        /// <summary>
        /// 是否已选择雪橇标的
        /// </summary>
        public bool HasSelectedXqTarget
        {
            get { return hasSelectedXqTarget; }
            set { SetProperty(ref hasSelectedXqTarget, value); }
        }
        
        private UserComposeViewContainer targetComposeUserComposeViewContainer;
        /// <summary>
        /// 所选组合标的的用户组合视图容器
        /// </summary>
        public UserComposeViewContainer TargetComposeUserComposeViewContainer
        {
            get { return targetComposeUserComposeViewContainer; }
            set { SetProperty(ref targetComposeUserComposeViewContainer, value); }
        }

        private TargetContract_TargetContractDetail targetContractDetailContainer;
        /// <summary>
        /// 所选合约标的的容器
        /// </summary>
        public TargetContract_TargetContractDetail TargetContractDetailContainer
        {
            get { return targetContractDetailContainer; }
            set { SetProperty(ref targetContractDetailContainer, value); }
        }

        private ICommand triggerSelectComposeCmd;
        public ICommand TriggerSelectComposeCmd
        {
            get { return triggerSelectComposeCmd; }
            set { SetProperty(ref triggerSelectComposeCmd, value); }
        }

        private ICommand triggerSelectContractCmd;
        public ICommand TriggerSelectContractCmd
        {
            get { return triggerSelectContractCmd; }
            set { SetProperty(ref triggerSelectContractCmd, value); }
        }

        private object xqTargetClosePositionHistoryContentView;
        /// <summary>
        /// 配对历史内容视图
        /// </summary>
        public object XqTargetClosePositionHistoryContentView
        {
            get { return xqTargetClosePositionHistoryContentView; }
            set { SetProperty(ref xqTargetClosePositionHistoryContentView, value); }
        }

    }
}
