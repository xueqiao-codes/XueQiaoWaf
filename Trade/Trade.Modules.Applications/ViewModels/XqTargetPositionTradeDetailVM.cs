using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqTargetPositionTradeDetailVM : ViewModel<IXqTargetPositionTradeDetailView>
    {
        [ImportingConstructor]
        public XqTargetPositionTradeDetailVM(IXqTargetPositionTradeDetailView view) : base(view)
        {
            PositionTradeDetailItems = new ObservableCollection<PositionTradeDetailItemDM>();
        }

        private XqTargetDM xqTargetItem;
        /// <summary>
        /// 雪橇标的 data model item
        /// </summary>
        public XqTargetDM XqTargetItem
        {
            get { return xqTargetItem; }
            set { SetProperty(ref xqTargetItem, value); }
        }

        private XqTargetDetailPositionDM detailPositionItem;
        /// <summary>
        /// 明细持仓项 data model item
        /// </summary>
        public XqTargetDetailPositionDM DetailPositionItem
        {
            get { return detailPositionItem; }
            set { SetProperty(ref detailPositionItem, value); }
        }

        public ObservableCollection<PositionTradeDetailItemDM> PositionTradeDetailItems { get; private set; }
        
        public void InvalidateViewWithPositionXqTargetType(ClientXQOrderTargetType orderTargetType)
        {
            ViewCore.UpdateListColumnsByPresentTarget(orderTargetType);
        }
    }
}
