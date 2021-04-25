using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 投机合约行情缓存 controller
    /// </summary>
    public interface ISpecQuotationCacheController 
    {
        void CacheQuotations(params NativeQuotationItem[] quotations);

        IEnumerable<NativeQuotationItem> GetCachedQuotationsBySymbol(string symbol);

        Dictionary<string, IEnumerable<NativeQuotationItem>> GetAllCachedQuotations();
        
    }
}
