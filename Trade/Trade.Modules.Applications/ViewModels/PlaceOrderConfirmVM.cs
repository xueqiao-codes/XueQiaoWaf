using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaceOrderConfirmVM : ViewModel<IPlaceOrderConfirmView>
    {
        [ImportingConstructor]
        public PlaceOrderConfirmVM(IPlaceOrderConfirmView view) : base(view)
        {
        }

        /// <summary>
        /// 下次是否不再提示确认
        /// </summary>
        private bool notConfirmNextTime;
        public bool NotConfirmNextTime
        {
            get { return notConfirmNextTime; }
            set { SetProperty(ref notConfirmNextTime, value); }
        }

        /// <summary>
        /// 订单的标的
        /// </summary>
        private IXqTargetDM orderTarget;
        public IXqTargetDM OrderTarget
        {
            get { return orderTarget; }
            set { SetProperty(ref orderTarget, value); }
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        private HostingXQOrderType orderType;
        public HostingXQOrderType OrderType
        {
            get { return orderType; }
            set { SetProperty(ref orderType, value); }
        }
        
        /// <summary>
        /// 订单操作账户相关数据
        /// </summary>
        private SubAccountFieldsForTradeData orderSubAccountFields;
        public SubAccountFieldsForTradeData OrderSubAccountFields
        {
            get { return orderSubAccountFields; }
            set { SetProperty(ref orderSubAccountFields, value); }
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        private HostingXQOrderDetail orderDetail;
        public HostingXQOrderDetail OrderDetail
        {
            get { return orderDetail; }
            set { SetProperty(ref orderDetail, value); }
        }
        
    }
}
