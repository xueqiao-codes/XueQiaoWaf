using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting.arbitrage.thriftapi;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 订单触发的显示类型
    /// </summary>
    public enum XQOrderTriggerDisplayType
    {
        TextDescription = 1,    // 文字描述类型
        TriggerNeedConfig = 2,  // 触发器需配置类型
    }

    /// <summary>
    /// 订单触发项目的显示 model
    /// </summary>
    public class XQOrderTriggerDisplayModel : Model
    {
        public XQOrderTriggerDisplayModel(XQOrderTriggerDisplayType triggerDisplayType)
        {
            this.SupportConfigTriggerPriceTypes = new ObservableCollection<HostingXQConditionTriggerPriceType>();

            this.TriggerDisplayType = triggerDisplayType;
            this.ConfigTrigger = new HostingXQConditionTrigger { ConditionPrice = 0 };

            EnumHelper.GetAllTypesForEnum(typeof(HostingXQConditionTriggerPriceType), out IEnumerable<HostingXQConditionTriggerPriceType> allTriggerPriceTypes);
            this.SupportConfigTriggerPriceTypes.AddRange(allTriggerPriceTypes);
            this.ConfigTrigger.TriggerPriceType = allTriggerPriceTypes.FirstOrDefault();
        }

        public XQOrderTriggerDisplayType TriggerDisplayType { get; private set; }

        /// <summary>
        /// 文字描述类型触发器的文字描述
        /// </summary>
        public string TextTriggerDescription { get; set; }

        /// <summary>
        /// 配置类型的触发器
        /// </summary>
        public HostingXQConditionTrigger ConfigTrigger { get; private set; }

        /// <summary>
        /// 配置类型的触发器支持的价格类型。默认全部的 <see cref="HostingXQConditionTriggerPriceType"/>都支持
        /// </summary>
        public ObservableCollection<HostingXQConditionTriggerPriceType> SupportConfigTriggerPriceTypes { get; private set; }

    }

    /// <summary>
    /// 下单视图创建剧本基类
    /// </summary>
    public abstract class PlaceOrderViewCreateDramaBase : Model
    {
        protected PlaceOrderViewCreateDramaBase(ClientPlaceOrderType viewPlaceOrderType,
            ClientXQOrderTargetType viewOrderTargetType)
        {
            this.SupportPriceTypeValues = new ObservableCollection<HostingXQOrderPrice>();
            this.SupportEffectDateTypeValues = new ObservableCollection<HostingXQEffectDate>();

            this.ViewPlaceOrderType = viewPlaceOrderType;
            this.ViewOrderTargetType = viewOrderTargetType;
            this.InitDatas();
        }
        
        /// <summary>
        /// 该视图的下单类型
        /// </summary>
        public ClientPlaceOrderType ViewPlaceOrderType { get; private set; }

        /// <summary>
        /// 该视图下单的标的类型
        /// </summary>
        public ClientXQOrderTargetType ViewOrderTargetType { get; private set; }
        
        /// <summary>
        /// 该视图支持的价格类型
        /// </summary>
        public ObservableCollection<HostingXQOrderPrice> SupportPriceTypeValues { get; private set; }

        private HostingXQOrderPrice selectedPriceTypeValue;
        /// <summary>
        /// 选中的价格类型
        /// </summary>
        public HostingXQOrderPrice SelectedPriceTypeValue
        {
            get { return selectedPriceTypeValue; }
            set { SetProperty(ref selectedPriceTypeValue, value); }
        }
        
        /// <summary>
        /// 该视图支持的有效期列表
        /// </summary>
        public ObservableCollection<HostingXQEffectDate> SupportEffectDateTypeValues { get; private set; }

        private HostingXQEffectDate selectedEffectDateTypeValue;
        /// <summary>
        /// 选中的有效期
        /// </summary>
        public HostingXQEffectDate SelectedEffectDateTypeValue
        {
            get { return selectedEffectDateTypeValue; }
            set { SetProperty(ref selectedEffectDateTypeValue, value); }
        }

        /// <summary>
        /// 订单触发显示 model
        /// </summary>
        public XQOrderTriggerDisplayModel OrderTriggerDisplayModel { get; protected set; }

        /// <summary>
        /// 可下单时段的文字描述
        /// </summary>
        public string PlaceOrderAvailableTimeDescription { get; protected set; }

        /// <summary>
        /// 是否显示买入按钮区域视图。默认 true
        /// </summary>
        public bool ShowBuyButtonAreaView { get; protected set; } = true;
        
        /// <summary>
        /// 是否显示卖出按钮区域视图。默认 true
        /// </summary>
        public bool ShowSellButtonAreaView { get; protected set; } = true;
        
        /// <summary>
        /// 验证并生成订单详情。返回该生成的订单详情、订单类型、验证错误信息
        /// </summary>
        /// <param name="outOrderDetail">返回生成的订单</param>
        /// <param name="outHostingOrderType">返回该订单的类型</param>
        /// <param name="outOrderValidateErrorMsg">如果验证不通过，返回验证错误信息</param>
        /// <param name="orderQuantity">下单数量</param>
        /// <param name="execEveryQty">单次发单数量</param>
        /// <param name="orderDirection">下单方向</param>
        /// <returns></returns>
        public abstract void ValidateAndGenerateOrderDetail(out HostingXQOrderDetail outOrderDetail, 
            out HostingXQOrderType? outHostingOrderType, 
            out string outOrderValidateErrorMsg,
            int orderQuantity, 
            HostingXQTradeDirection orderDirection);

        /// <summary>
        /// 初始化数据
        /// </summary>
        protected abstract void InitDatas();

        
    }
}
