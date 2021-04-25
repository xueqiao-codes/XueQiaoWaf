using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 用户设置数据树包
    /// </summary>
    public class UserSettingDataTreesPackage 
    {
        /// <summary>
        /// 交易组件中的列表列信息数据树
        /// </summary>
        public TradeComponentListColumnInfosDataTree TradeComponentListColumnInfosDT { get; set; }

        /// <summary>
        /// 交易工作空间模板数据树
        /// </summary>
        public TradeWorkspaceTemplateDataTree TradeWorkspaceTemplateDT { get; set; }

        /// <summary>
        /// 交易工作空间的窗口数据树
        /// </summary>
        public WorkspaceWindowTree TradeWorkspaceWindowTree { get; set; }

        /// <summary>
        /// 交易工作空间详情列表
        /// </summary>
        public IEnumerable<TradeWorkspaceItemTree> TradeWorkspaceList { get; set; }

        /// <summary>
        /// 订阅数据树
        /// </summary>
        public UserSubscribeDataTree SubscribeDataTree { get; set; }

        /// <summary>
        /// 结算的雪橇成交预览数据树
        /// </summary>
        public XqTPIDataTree XqTPIDataTree { get; set; }

        /// <summary>
        /// 持仓预分配数据树
        /// </summary>
        public PPADataTree PPADataTree { get; set; }

        /// <summary>
        /// 雪橇组合订单执行参数模板数据树 
        /// </summary>
        public XQComposeOrderEPTDataTree XQComposeOrderEPTDataTree { get; set; }

        /// <summary>
        /// 投研工作空间的窗口数据树
        /// </summary>
        public WorkspaceWindowTree ResearchWorkspaceWindowTree { get; set; }

        /// <summary>
        /// 投研工作空间详情列表
        /// </summary>
        public IEnumerable<ResearchWorkspaceItemTree> ResearchWorkspaceList { get; set; }

    }
}
