using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionAssistantComponentContainerModel : ViewModel<IPositionAssistantComponentContainer>
    {
        [ImportingConstructor]
        protected PositionAssistantComponentContainerModel(IPositionAssistantComponentContainer view) : base(view)
        {
            EnumHelper.GetAllTypesForEnum(typeof(PositionAssistantContentType), out IEnumerable<PositionAssistantContentType> types);
            this.assistantContentTypes = types;
        }

        private readonly IEnumerable<PositionAssistantContentType> assistantContentTypes;
        public PositionAssistantContentType[] AssistantContentTypes
        {
            get
            {
                return assistantContentTypes.ToArray();
            }
        }

        private ICommand toInputComposePositionCmd;
        public ICommand ToInputComposePositionCmd
        {
            get { return toInputComposePositionCmd; }
            set { SetProperty(ref toInputComposePositionCmd, value); }
        }

        private ICommand toMerge2ComposePositionCmd;
        public ICommand ToMerge2ComposePositionCmd
        {
            get { return toMerge2ComposePositionCmd; }
            set { SetProperty(ref toMerge2ComposePositionCmd, value); }
        }

        private ICommand showClosePositionSearchPageCmd;
        public ICommand ShowClosePositionSearchPageCmd
        {
            get { return showClosePositionSearchPageCmd; }
            set { SetProperty(ref showClosePositionSearchPageCmd, value); }
        }
        
        private PositionAssistantContentType selectedContentType;
        public PositionAssistantContentType SelectedContentType
        {
            get { return selectedContentType; }
            set { SetProperty(ref selectedContentType, value); }
        }
        
        private object assistantContentView;
        public object AssistantContentView
        {
            get { return assistantContentView; }
            set { SetProperty(ref assistantContentView, value); }
        }
    }


    // 持仓助手内容类型
    public enum PositionAssistantContentType
    {
        CONTRACT = 1,       // 投机持仓
        COMPOSE = 2,        // 组合持仓
    }
}
