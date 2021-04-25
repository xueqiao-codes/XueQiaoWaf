using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 货币单位服务
    /// </summary>
    public interface ICurrencyUnitsService
    {
        /// <summary>
        /// 支持货币的单位信息列表
        /// </summary>
        IEnumerable<CurrencyUnitInfo> SupportCurrencyUnitInfos { get; }

        /// <summary>
        /// 获取某个货币的单位信息
        /// </summary>
        CurrencyUnitInfo GetCurrencyUnitInfo(string currency);

        /// <summary>
        /// 获取货币单位名称
        /// </summary>
        /// <param name="currency">货币</param>
        /// <param name="currencyChargeUnit">货币计价单位，如1, 0.1, 0.01, 0.001</param>
        /// <returns></returns>
        string GetCurrencyUnitName(string currency, double currencyChargeUnit);
    }
}
