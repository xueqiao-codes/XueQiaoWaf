using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    /// <summary>
    /// 订阅的组合 data model
    /// </summary>
    public class SubscribeComposeDataModel : Model, IMarketSubscribeData
    {
        /// <summary>
        /// 共享的组合列表的组合 Group key
        /// </summary>
        public const string SharedListComposeGroupKey = "2fad7d6a-e080-11e7-80c1-9a214cf093ae";

        /// <summary>
        /// 获取唯一的订阅组合 Group key
        /// </summary>
        /// <returns></returns>
        public static String UniqueSubscribeComposeGroupKey()
        {
            return Guid.NewGuid().ToString();
        }

        public SubscribeComposeDataModel(long composeId, string subscribeGroupKey)
        {
            if (subscribeGroupKey == null) throw new ArgumentNullException("subscribeGroupKey can't be null.");
            this.ComposeId = composeId;
            this.SubscribeGroupKey = subscribeGroupKey;
            this.ComposeDetailContainer = new TargetCompose_ComposeDetail(composeId);
            this.UserComposeViewContainer = new UserComposeViewContainer(composeId);
        }

        /// <summary>
        /// 组合 id
        /// </summary>
        public long ComposeId { get; private set; }

        /// <summary>
        /// 用户 id
        /// </summary>
        public int SubUserId { get; private set; }

        /// <summary>
        /// 订阅组 key
        /// </summary>
        public string SubscribeGroupKey { get; private set; }
        
        /// <summary>
        /// 组合详情容器
        /// </summary>
        public TargetCompose_ComposeDetail ComposeDetailContainer { get; private set; }

        /// <summary>
        /// 用户组合视图容器
        /// </summary>
        public UserComposeViewContainer UserComposeViewContainer { get; private set; }

        private MarketSubscribeState subscribeState;
        public MarketSubscribeState SubscribeState
        {
            get { return subscribeState; }
            set { SetProperty(ref subscribeState, value); }
        }

        private string subscribeStateMsg;
        public string SubscribeStateMsg
        {
            get { return subscribeStateMsg; }
            set { SetProperty(ref subscribeStateMsg, value); }
        }

        private NativeQuotationItem combQuotation;
        // 组合行情
        public NativeQuotationItem CombQuotation
        {
            get { return combQuotation; }
            set { SetProperty(ref combQuotation, value); }
        }

        // 腿行情列表
        private ObservableCollection<ComposeLegQuotationDM> legQuotations;
        public ObservableCollection<ComposeLegQuotationDM> LegQuotations
        {
            get { return legQuotations; }
            set { SetProperty(ref legQuotations, value); }
        }

        private IEnumerable<string> customGroupKeys;
        // 自定义分组列表
        public IEnumerable<string> CustomGroupKeys
        {
            get { return customGroupKeys; }
            set { SetProperty(ref customGroupKeys, value); }
        }
        
        private IEnumerable<long> onTradingSubAccountIds;
        /// <summary>
        /// 该组合正进行交易中的操作账户id列表
        /// </summary>
        public IEnumerable<long> OnTradingSubAccountIds
        {
            get { return onTradingSubAccountIds; }
            set { SetProperty(ref onTradingSubAccountIds, value); }
        }

        private IEnumerable<long> existPositionSubAccountIds;
        /// <summary>
        /// 该组合存在持仓的子账户id列表
        /// </summary>
        public IEnumerable<long> ExistPositionSubAccountIds
        {
            get { return existPositionSubAccountIds; }
            set { SetProperty(ref existPositionSubAccountIds, value); }
        }

        private long createTimestamp;
        public long CreateTimestamp
        {
            get { return createTimestamp; }
            set { SetProperty(ref createTimestamp, value); }
        }

        private bool isXqTargetExpired;
        /// <summary>
        /// 标的是否已过期
        /// </summary>
        public bool IsXqTargetExpired
        {
            get { return isXqTargetExpired; }
            set { SetProperty(ref isXqTargetExpired, value); }
        }
        
        private string xqTargetName;
        /// <summary>
        /// 标的名称（第一层级，方便用于列表排序）
        /// </summary>
        public string XqTargetName
        {
            get { return xqTargetName; }
            set { SetProperty(ref xqTargetName, value); }
        }
    }
}
