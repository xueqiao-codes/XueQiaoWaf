using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.DataModels
{
    /// <summary>
    /// 交易模块的工作空间数据根
    /// </summary>
    public class TradeWorkspaceDataRoot
    {
        public TradeWorkspaceDataRoot()
        {
            TradeWorkspaceTemplateListContainer = new TradeTabWorkspaceTemplateListContainer();
            InterTabWorkspaceWindowListContainer = new InterTabWorkspaceWindowListContainer();
            MainWindowWorkspaceListContainer = new TabWorkspaceListContainer();
        }

        /// <summary>
        /// 全局应用的合约列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedContractListDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的组合列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedComposeListDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的委托单列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedOrderListEntrustedDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的预埋单列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedOrderListParkedDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的条件单列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedOrderListConditionDisplayColumns { get; set; }

        /// <summary>
        /// 全局应用的成交列表显示的列
        /// </summary>
        public ListColumnInfo[] GlobalAppliedTradeListDisplayColumns { get; set; }

        /// <summary>
        /// 交易工作空间模板
        /// </summary>
        public TradeTabWorkspaceTemplateListContainer TradeWorkspaceTemplateListContainer { get; private set; }

        /// <summary>
        /// 拆分窗口列表容器
        /// </summary>
        public InterTabWorkspaceWindowListContainer InterTabWorkspaceWindowListContainer { get; private set; }

        /// <summary>
        /// 主窗口的工作空间容器
        /// </summary>
        public TabWorkspaceListContainer MainWindowWorkspaceListContainer { get; private set; }
    }
    
    /// <summary>
    /// 交易工作空间模板列表容器
    /// </summary>
    public class TradeTabWorkspaceTemplateListContainer
    {
        public TradeTabWorkspaceTemplateListContainer()
        {
            Templates = new ObservableCollection<XueQiaoFoundation.BusinessResources.DataModels.TradeTabWorkspaceTemplate>();
        }

        public ObservableCollection<XueQiaoFoundation.BusinessResources.DataModels.TradeTabWorkspaceTemplate> Templates { get; private set; }
    } 
}
