using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;
using XueQiaoWaf.Trade.Interfaces.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Applications
{
    /// <summary>
    /// 交易模块协议
    /// </summary>
    public interface ITradeModuleService
    {
        /// <summary>
        /// 生成雪橇订单 id。
        /// </summary>
        string GenerateXQOrderId(long subAccountId);
        
        /// <summary>
        /// 专门用于订单操作请求（具体为：下单、撤单）的任务工厂
        /// </summary>
        TaskFactory OrderOperateRequestTaskFactory { get; }
        
        /// <summary>
        /// 获取`交易`模块的根视图
        /// </summary>
        /// <param name="embedInWindowCaptionDataHolderGetter">视图嵌入到窗体头部时的数据 holder 的获取方法</param>
        /// <param name="showAction">切换到显示该 tab 时的回调</param>
        /// <param name="closeAction">切换到关闭该 tab 的回调</param>
        /// <returns></returns>
        object GetTradeModuleRootView(Func<ChromeWindowCaptionDataHolder> embedInWindowCaptionDataHolderGetter,
            out Action showAction, out Action closeAction);

        /// <summary>
        /// 交易工作空间数据根
        /// </summary>
        TradeWorkspaceDataRoot TradeWorkspaceDataRoot { get; }

        /// <summary>
        /// 订阅数据分组数据根
        /// </summary>
        SubscribeDataGroupsDataRoot SubscribeDataGroupsDataRoot { get; }

        /// <summary>
        /// 生成交易模块的工作空间 unique key
        /// </summary>
        string GenerateTradeWorkspaceKey();
    }
}
