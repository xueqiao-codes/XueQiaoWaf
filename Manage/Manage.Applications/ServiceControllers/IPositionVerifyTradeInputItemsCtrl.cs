using Manage.Applications.DataModels;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Applications.ServiceControllers
{
    /// <summary>
    /// 结算的雪橇预览成交项 controller
    /// </summary>
    internal interface IPositionVerifyTradeInputItemsCtrl
    {
        /// <summary>
        /// 添加或修改预览成交项。
        /// </summary>
        /// <param name="priviewItemKey">预览成交项id</param>
        /// <param name="fundAccountId">资金账号id</param>
        /// <param name="belongVerifyDailySec">所属核算日期时间点</param>
        /// <param name="updateTemplateFactory">修改模板工厂。arg1：是否存在该priviewItemKey的项，arg2：返回的模板</param>
        /// <returns></returns>
        PositionVerifyTradeInputDM AddOrUpdateItem(string priviewItemKey, long fundAccountId, long? belongVerifyDailySec,
            Func<bool, XqPreviewInputTradeItemUpdateTemplate> updateTemplateFactory);

        /// <summary>
        /// 修改预览成交项
        /// </summary>
        /// <param name="priviewItemKey"></param>
        /// <param name="updateTemplateFactory"></param>
        void UpdateItem(string priviewItemKey, Func<XqPreviewInputTradeItemUpdateTemplate> updateTemplateFactory);

        /// <summary>
        /// 删除预览成交项
        /// </summary>
        /// <param name="priviewItemKey"></param>
        void RemoveItem(string priviewItemKey);
        
        /// <summary>
        /// 获取所有预览成交项
        /// </summary>
        /// <returns></returns>
        IEnumerable<PositionVerifyTradeInputDM> AllItems { get; }
    }

    internal class XqPreviewInputTradeItemUpdateTemplate
    {
        public int? ContractId { get; set; }

        public long? TradeTimestamp { get; set; }

        public ClientTradeDirection? Direction { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }
    }
}
