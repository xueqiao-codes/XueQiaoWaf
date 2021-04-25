using lib.xqclient_base.thriftapi_mediation.Interface;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.ServiceControllers
{
    /// <summary>
    /// 订阅组合管理
    /// </summary>
    public interface ISubscribeComposeController
    {
        /// <summary>
        /// 添加或修改某个订阅合约
        /// </summary>
        /// <param name="composeId">组合 id</param>
        /// <param name="subscribeGroupKey">订阅 group key</param>
        /// <param name="sameComposeIdItemsUpdateAction">相同组合id的订阅组合项修改方法。arg1:是否存在该新加的订阅组合，arg2:返回与新加的id相同的订阅组合的修改模板</param>
        SubscribeComposeDataModel AddOrUpdateSubscribeCompose(long composeId, 
            string subscribeGroupKey, 
            Func<bool, SubscribeComposeUpdateTemplate> sameComposeIdItemsUpdateAction);

        /// <summary>
        /// 删除某个订阅组合
        /// </summary>
        /// <param name="composeId">组合 id</param>
        /// <param name="subscribeGroupKey">订阅 group key。当subscribeGroupKey为 null 时，删除该组合 id 的所有订阅</param>
        void RemoveSubscribeCompose(long composeId, string subscribeGroupKey = null);

        /// <summary>
        /// 修改相同 id 的订阅组合
        /// </summary>
        /// <param name="composeId">组合 id</param>
        /// <param name="updateAction">修改方法。返回修改模板</param>
        void UpdateSubscribeComposesWithSameId(long composeId, Func<SubscribeComposeDataModel, SubscribeComposeUpdateTemplate> updateAction);

        /// <summary>
        /// 获取当前用户订阅的所有key是shared的组合
        /// </summary>
        /// <returns></returns>
        IEnumerable<SubscribeComposeDataModel> GetSharedGroupKeySubscribeComposes();
        
        /// <summary>
        /// 订阅组合的行情
        /// </summary>
        /// <param name="composeId"></param>
        /// <returns>订阅操作结果</returns>
        Task<CombQuotationSubscribeInteractInfo> SubscribeCombQuotationAsync(long composeId);

        /// <summary>
        /// 退订组合的行情
        /// </summary>
        /// <param name="composeId"></param>
        /// <returns>订阅操作结果</returns>
        Task<CombQuotationSubscribeInteractInfo> UnsubscribeCombQuotationAsync(long composeId);
        
        /// <summary>
        /// 在所有组合中是否存在某个合约腿
        /// </summary>
        /// <param name="legContractId"></param>
        /// <returns></returns>
        bool ExistLegContractInCurrentComposes(int legContractId);

        /// <summary>
        /// 获取指定 composeId 的订阅项列表  
        /// </summary>
        /// <param name="composeId"></param>
        /// <returns></returns>
        IEnumerable<SubscribeComposeDataModel> GetSubscribeItem(long composeId);

        /// <summary>
        /// 获取订阅项
        /// </summary>
        /// <param name="composeId"></param>
        /// <param name="groupKey"></param>
        /// <returns></returns>
        SubscribeComposeDataModel GetSubscribeItem(long composeId, string groupKey);
    }

    /// <summary>
    /// 订阅的组合修改模板
    /// </summary>
    public class SubscribeComposeUpdateTemplate
    {
        /// <summary>
        /// 要修改的组合行情信息
        /// </summary>
        public Tuple<NativeQuotationItem> CombQuotation;

        /// <summary>
        /// 要修改的腿行情信息
        /// </summary>
        public Tuple<IEnumerable<NativeQuotationItem>> LegQuotationItems;

        /// <summary>
        /// 要修改的订阅状态
        /// </summary>
        public Tuple<MarketSubscribeState> SubscribeState;

        /// <summary>
        /// 要修改的订阅信息
        /// </summary>
        public Tuple<string> SubscribeStateMsg;

        /// <summary>
        /// 自定义分组列表
        /// </summary>
        public Tuple<IEnumerable<string>> CustomGroupKeys;

        /// <summary>
        /// 该组合正进行交易中的子账户id列表
        /// </summary>
        public Tuple<IEnumerable<long>> OnTradingSubAccountIds;

        /// <summary>
        /// 该组合存在持仓的子账户id列表
        /// </summary>
        public Tuple<IEnumerable<long>> ExistPositionSubAccountIds;

    }

    /// <summary>
    /// 组合行情订阅操作的交互信息
    /// </summary>
    public class CombQuotationSubscribeInteractInfo
    {
        /// <summary>
        /// 是否请求了操作 api
        /// </summary>
        public bool HasRequestApi;

        /// <summary>
        /// 操作 api 返回信息
        /// </summary>
        public IInterfaceInteractResponse ApiResponse;
    }
}
