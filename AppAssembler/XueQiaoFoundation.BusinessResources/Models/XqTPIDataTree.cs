using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 雪橇成交预录入(Xq Trade Preview Input)数据树
    /// </summary>
    public class XqTPIDataTree
    {
        public XqTPIItem[] XqTPIItems { get; set; }
    }

    /// <summary>
    /// 雪橇成交预录入项
    /// </summary>
    public class XqTPIItem
    {
        // Item 唯一标识
        public string Key { get; set; }

        // 资金账号 id
        public long FAccId { get; set; }
        
        // 合约 id
        public int? CId { get; set; }

        // 成交时间戳
        public long? TradeTS { get; set; }

        // 方向
        public int? Dir { get; set; }

        // 价格
        public double? Price { get; set; }

        // 数量
        public int? Quantity { get; set; }

        // 所属核算日期时间点
        public long? VerifyDailySec { get; set; }
    }
}
