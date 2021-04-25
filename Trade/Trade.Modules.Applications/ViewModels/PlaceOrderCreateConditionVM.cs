using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaceOrderCreateConditionVM : ViewModel<IPlaceOrderCreateConditionView>
    {
        private readonly XQComposeOrderEPTService _XQComposeOrderEPTService;

        [ImportingConstructor]
        protected PlaceOrderCreateConditionVM(IPlaceOrderCreateConditionView view,
            XQComposeOrderEPTService _XQComposeOrderEPTService) : base(view)
        {
            this._XQComposeOrderEPTService = _XQComposeOrderEPTService;
            this.ComposeOrderEPTs = new SynchronizingCollection<XQComposeOrderExecParamsTemplate, XQComposeOrderExecParamsTemplate>(_XQComposeOrderEPTService.ArchivedEPTs, i => i);
        }
        
        private PlaceOrderViewCreateDramaBase placeOrderViewCreateDrama;
        /// <summary>
        /// 下单视图创建剧本
        /// </summary>
        public PlaceOrderViewCreateDramaBase PlaceOrderViewCreateDrama
        {
            get { return placeOrderViewCreateDrama; }
            set { SetProperty(ref placeOrderViewCreateDrama, value); }
        }
        
        /// <summary>
        /// 雪橇组合订单执行参数模板列表
        /// </summary>
        public ReadOnlyObservableCollection<XQComposeOrderExecParamsTemplate> ComposeOrderEPTs { get; private set; }

        private XQComposeOrderExecParamsTemplate selectedComposeOrderEPT;
        public XQComposeOrderExecParamsTemplate SelectedComposeOrderEPT
        {
            get { return selectedComposeOrderEPT; }
            set { SetProperty(ref selectedComposeOrderEPT, value); }
        }

        private ICommand toComposeOrderEPTManageCmd;
        /// <summary>
        /// 打开组合标的订单执行参数模板管理页面 command
        /// </summary>
        public ICommand ToComposeOrderEPTManageCmd
        {
            get { return toComposeOrderEPTManageCmd; }
            set { SetProperty(ref toComposeOrderEPTManageCmd, value); }
        }
    }
}
