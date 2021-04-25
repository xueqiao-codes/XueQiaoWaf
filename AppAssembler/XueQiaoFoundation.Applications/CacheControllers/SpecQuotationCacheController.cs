using NativeModel.Trade;
using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.quotation;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoFoundation.Interfaces.Event.business;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;


namespace XueQiaoFoundation.Applications.CacheControllers
{
    [Export(typeof(ISpecQuotationCacheController)), Export(typeof(IXueQiaoFoundationSingletonController))]
    internal class SpecQuotationCacheController : ISpecQuotationCacheController, IXueQiaoFoundationSingletonController,
        IInternalCacheController<string>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly ConcurrentDictionary<string, List<NativeQuotationItem>> cachedQuotationListDictionary;
        
        [ImportingConstructor]
        public SpecQuotationCacheController(IEventAggregator eventAggregator,
            Lazy<ILoginUserManageService> loginUserManageService)
        {
            this.eventAggregator = eventAggregator;
            this.loginUserManageService = loginUserManageService;

            cachedQuotationListDictionary = new ConcurrentDictionary<string, List<NativeQuotationItem>>();

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
            this.eventAggregator.GetEvent<SpecQuotationUpdated>().Subscribe(RecvSpecQuotationUpdateEvent, ThreadOption.PublisherThread);
        }

        public void Shutdown()
        {
            ClearAllCaches();

            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            this.eventAggregator.GetEvent<SpecQuotationUpdated>().Unsubscribe(RecvSpecQuotationUpdateEvent);
        }
        
        public void CacheQuotations(params NativeQuotationItem[] quotations)
        {
            // TODO: 缓存起行情，暂时不缓存
            //if (quotations == null) return;
            //var symbolGroupedQuots = quotations.GroupBy(i => i.ContractSymbol);
            //foreach (var groupItem in symbolGroupedQuots)
            //{
            //    var sortedQuots = groupItem.OrderBy(i => i.RaceTimestampMs).ToArray();
            //    cachedQuotationListDictionary.AddOrUpdate(groupItem.Key, new List<QuotationItem>(sortedQuots),
            //        (symbol, _list) => 
            //        {
            //            var lastQuot = _list.LastOrDefault();
            //            _list.AddRange(sortedQuots.Where(i =>
            //            {
            //                if (lastQuot == null) return true;
            //                return i.RaceTimestampMs > lastQuot.RaceTimestampMs;
            //            }));
            //            return _list;
            //        });
            //}
        }

        public IEnumerable<NativeQuotationItem> GetCachedQuotationsBySymbol(string symbol)
        {
            if (symbol == null) return null;
            if (cachedQuotationListDictionary.TryGetValue(symbol, out List<NativeQuotationItem> _list))
            {
                return _list.ToArray();
            }
            return null;
        }

        public Dictionary<string, IEnumerable<NativeQuotationItem>> GetAllCachedQuotations()
        {
            return cachedQuotationListDictionary.ToArray().ToDictionary(i=>i.Key,i=>i.Value.AsEnumerable());
        }

        public void RemoveCache(string symbol)
        {
            if (symbol == null) return;
            cachedQuotationListDictionary.TryRemove(symbol, out List<NativeQuotationItem> ignore);
        }

        public void ClearAllCaches()
        {
            cachedQuotationListDictionary.Clear();
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            ClearAllCaches();
        }

        private void RecvSpecQuotationUpdateEvent(NativeQuotationItem quotation)
        {
            CacheQuotations(quotation);
        }
    }
}
