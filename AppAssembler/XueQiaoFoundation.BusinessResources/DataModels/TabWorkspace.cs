using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class TabWorkspace : Model
    {
        public TabWorkspace(string workspaceKey)
        {
            this.WorkspaceKey = workspaceKey;
            TradeComponents = new ObservableCollection<TradeComponent>();
            ResearchComponents = new ObservableCollection<ResearchComponent>();
        }

        /// <summary>
        /// WorkspaceKey
        /// </summary>
        public string WorkspaceKey { get; private set; }

        /// <summary>
        /// 工作空间类型，取值范围 <see cref="XueQiaoFoundation.BusinessResources.Models.WorkspaceTypeReference"/>
        /// </summary>
        public int WorkspaceType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        /// <summary>
        /// 描述
        /// </summary>
        private string workspaceDesc;
        public string WorkspaceDesc
        {
            get { return workspaceDesc; }
            set { SetProperty(ref workspaceDesc, value); }
        }

        /// <summary>
        /// 排列顺序
        /// </summary>
        private int order;
        public int Order
        {
            get { return order; }
            set { SetProperty(ref order, value); }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        private long subAccountId;
        /// <summary>
        /// 子账户 id
        /// </summary>
        public long SubAccountId
        {
            get { return subAccountId; }
            set { SetProperty(ref subAccountId, value); }
        }
        
        /// <summary>
        /// 交易组件
        /// </summary>
        public ObservableCollection<TradeComponent> TradeComponents { get; private set; }

        /// <summary>
        /// 投研组件
        /// </summary>
        public ObservableCollection<ResearchComponent> ResearchComponents { get; private set; }

    }
}
