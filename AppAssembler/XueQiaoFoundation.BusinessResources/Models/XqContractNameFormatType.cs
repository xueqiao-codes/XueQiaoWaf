using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 合约名称格式化类型
    /// </summary>
    public enum XqContractNameFormatType
    {
        CommodityAcronym_Code_ContractCode = 0,     // 商品简称+商品code+合约code
        CommodityAcronym_ContractCode = 1,          // 商品简称+合约code
        CommodityCode_ContractCode = 2,             // 商品code+合约code
    }
}
