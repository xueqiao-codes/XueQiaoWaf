using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 交易所服务
    /// </summary>
    public interface IExchangeDataService
    {
        /// <summary>
        /// 是否是内盘交易所
        /// </summary>
        /// <param name="sledExchangeMic">交易所mic</param>
        /// <returns></returns>
        bool IsInnerExchange(string sledExchangeMic);

        /// <summary>
        /// 国内交易所 mic 列表
        /// </summary>
        string[] InnerExchangeMicList { get; }
        
        /// <summary>
        /// 优先的交易所 mic 列表
        /// </summary>
        string[] PreferredExchangeMicList { get; }

        /// <summary>
        /// 优先的交易所所属国家缩写列表
        /// </summary>
        string[] PreferredExchangeCountryAcronymList { get; }
    }
}
