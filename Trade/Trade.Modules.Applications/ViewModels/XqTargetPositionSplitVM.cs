using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqTargetPositionSplitVM : ViewModel<IXqTargetPositionSplitView>
    {
        [ImportingConstructor]
        protected XqTargetPositionSplitVM(IXqTargetPositionSplitView view) : base(view)
        {
            PositionSplitEditItems = new ObservableCollection<XqTargetPositionSplitEditItem>();
        }

        private TargetPositionDataModel xqTargetPositionItem;
        public TargetPositionDataModel XqTargetPositionItem
        {
            get { return xqTargetPositionItem; }
            set { SetProperty(ref xqTargetPositionItem, value); }
        }
        
        public ObservableCollection<XqTargetPositionSplitEditItem> PositionSplitEditItems { get; private set; }

        private ICommand triggerSplitPositionItemCmd;
        public ICommand TriggerSplitPositionItemCmd
        {
            get { return triggerSplitPositionItemCmd; }
            set { SetProperty(ref triggerSplitPositionItemCmd, value); }
        }
    }
}
