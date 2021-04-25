using business_foundation_lib.quotationpush;
using NativeModel.Contract;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Interfaces.Applications
{
    /// <summary>
    /// 行情 engine 管理
    /// </summary>
    public interface IQuotationEngineController
    {
        bool SubscribeContractQuotation(SubscribeQuotationKey subKey);

        bool UnsubscribeContractQuotation(SubscribeQuotationKey unsubKey);

        /// <summary>
        /// 行情服务是否已连接
        /// </summary>
        bool IsQuotationServerConnected { get; }
        
    }
}
