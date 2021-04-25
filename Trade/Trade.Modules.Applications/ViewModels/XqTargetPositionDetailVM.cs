using System;
using System.Collections.Generic;
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
    public class XqTargetPositionDetailVM : ViewModel<IXqTargetPositionDetailView>
    {
        [ImportingConstructor]
        protected XqTargetPositionDetailVM(IXqTargetPositionDetailView view) : base(view)
        {
        }

        private TargetPositionDataModel xqTargetPositionItem;
        public TargetPositionDataModel XqTargetPositionItem
        {
            get { return xqTargetPositionItem; }
            set { SetProperty(ref xqTargetPositionItem, value); }
        }
        
        private XqTargetPositionContentTabType selectedContentTabType;
        public XqTargetPositionContentTabType SelectedContentTabType
        {
            get { return selectedContentTabType; }
            set { SetProperty(ref selectedContentTabType, value); }
        }
        
        private object contentTabContentView;
        public object ContentTabContentView
        {
            get { return contentTabContentView; }
            set { SetProperty(ref contentTabContentView, value); }
        }
    }

    public enum XqTargetPositionContentTabType
    {
        PositionDetailTab = 1,
        HistoryClosePositionTab = 2
    }
}
