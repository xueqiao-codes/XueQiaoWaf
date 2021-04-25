using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XqOrderDetailViewModel : ViewModel<IXqOrderDetailView>
    {
        [ImportingConstructor]
        protected XqOrderDetailViewModel(IXqOrderDetailView view) : base(view)
        {
        }
        
        private OrderItemDataModel_Entrusted sourceOrderItem;
        public OrderItemDataModel_Entrusted SourceOrderItem
        {
            get { return sourceOrderItem; }
            set { SetProperty(ref sourceOrderItem, value); }
        }

        private ICommand toShowOrderExecuteParamsCmd;
        public ICommand ToShowOrderExecuteParamsCmd
        {
            get { return toShowOrderExecuteParamsCmd; }
            set { SetProperty(ref toShowOrderExecuteParamsCmd, value); }
        }

        private ICommand toShowRelatedOrderCmd;
        public ICommand ToShowRelatedOrderCmd
        {
            get { return toShowRelatedOrderCmd; }
            set { SetProperty(ref toShowRelatedOrderCmd, value); }
        }

        private XqOrderDetailContentTabType selectedContentTabType;
        public XqOrderDetailContentTabType SelectedContentTabType
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

    public enum XqOrderDetailContentTabType
    {
        TradeDetailTab = 1,     // 成交详情 tab
        ExecuteDetailTab = 2    // 执行详情 tab
    }
}
