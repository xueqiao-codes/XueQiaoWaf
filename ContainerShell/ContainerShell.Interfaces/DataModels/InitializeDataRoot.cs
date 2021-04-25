using NativeModel.Contract;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.config.contractlistrule.thriftapi;
using xueqiao.trade.hosting;
using XueQiaoFoundation.BusinessResources.Models;

namespace ContainerShell.Interfaces.DataModels
{
    /// <summary>
    /// 初始化数据汇总
    /// </summary>
    public class InitializeDataRoot : ICloneable
    {
        /// <summary>
        /// 登录用户的用户设置数据包
        /// </summary>
        public UserSettingDataTreesPackage LoginUserSettingDataTreePackage { get; set; }

        /// <summary>
        /// 登录用户的所有组合视图列表
        /// </summary>
        public IEnumerable<NativeComposeViewDetail> LoginUserAllComposeViews { get; set; }

        /// <summary>
        /// 所有交易所
        /// </summary>
        public IEnumerable<NativeExchange> AllExchanges { get; set; }

        /// <summary>
        /// 所有商品
        /// </summary>
        public IEnumerable<NativeCommodity> AllCommodities { get; set; }

        /// <summary>
        /// 登录用户的相关子账户
        /// </summary>
        public IEnumerable<HostingSubAccountRelatedItem> LoginUserRelatedSubAccountItems { get; set; }

        /// <summary>
        /// 登录的个人用户的订单路由树
        /// </summary>
        public HostingOrderRouteTree PersonalLoginUserOrderRouteTree { get; set; }

        /// <summary>
        /// 合约选择列表显示规则
        /// </summary>
        public IEnumerable<ContractListRule> ContractChooseListRules { get; set; }
        
        public object Clone()
        {
            return new InitializeDataRoot
            {
                LoginUserSettingDataTreePackage = this.LoginUserSettingDataTreePackage,
                LoginUserAllComposeViews = this.LoginUserAllComposeViews?.ToArray(),
                AllExchanges = this.AllExchanges?.ToArray(),
                AllCommodities = this.AllCommodities?.ToArray(),
                LoginUserRelatedSubAccountItems = this.LoginUserRelatedSubAccountItems?.ToArray(),
                PersonalLoginUserOrderRouteTree = this.PersonalLoginUserOrderRouteTree,
                ContractChooseListRules = this.ContractChooseListRules?.ToArray(),
            };
        }
    }
}
