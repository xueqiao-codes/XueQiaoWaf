using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 交易组件
    /// </summary>
    public class TradeComponent : XQComponentBase
    {
        public TradeComponent()
        {
            SubscribeDataContainerComponentDetail = new SubscribeDataContainerComponentDetail();
            PlaceOrderContainerComponentDetail = new PlaceOrderContainerComponentDetail();
            AccountContainerComponentDetail = new AccountContainerComponentDetail();
        }

        /// <summary>
        /// 交易组件的类型
        /// </summary>
        private int componentType;
        public int ComponentType
        {
            get { return componentType; }
            set { SetProperty(ref componentType, value); }
        }
        
        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool isLocked;
        public bool IsLocked
        {
            get { return isLocked; }
            set { SetProperty(ref isLocked, value); }
        }
        
        /// <summary>
        /// 组件描述标题
        /// </summary>
        private string componentDescTitle;
        public string ComponentDescTitle
        {
            get { return componentDescTitle; }
            set { SetProperty(ref componentDescTitle, value); }
        }
        
        /// <summary>
        /// 订阅数据容器组件详情
        /// </summary>
        public SubscribeDataContainerComponentDetail SubscribeDataContainerComponentDetail { get; set; }

        /// <summary>
        /// 下单容器组件详情
        /// </summary>
        public PlaceOrderContainerComponentDetail PlaceOrderContainerComponentDetail { get; set; }

        /// <summary>
        /// 账号容器组件详情
        /// </summary>
        public AccountContainerComponentDetail AccountContainerComponentDetail { get; set; }
    }
}
