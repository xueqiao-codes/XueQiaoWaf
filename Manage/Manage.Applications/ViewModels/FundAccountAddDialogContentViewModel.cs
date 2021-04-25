using Manage.Applications.DataModels;
using Manage.Applications.Domain;
using Manage.Applications.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using xueqiao.broker;
using XueQiaoFoundation.UI.Controls.AutoCompleteTextBox;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class FundAccountAddDialogContentViewModel : ViewModel<IFundAccountAddDialogContentView>
    {
        [ImportingConstructor]
        protected FundAccountAddDialogContentViewModel(IFundAccountAddDialogContentView view) : base(view)
        {
        }

        #region 改为券商输入自动完成模式
        private BrokerSelectNodeListSuggestionProvider brokerListSuggestionProvider;
        public BrokerSelectNodeListSuggestionProvider BrokerListSuggestionProvider
        {
            get { return brokerListSuggestionProvider; }
            private set { SetProperty(ref brokerListSuggestionProvider, value); }
        }

        public void UpdateBrokerNodeList(IEnumerable<BrokerSelectNode> brokerNodeList)
        {
            this.BrokerListSuggestionProvider = new BrokerSelectNodeListSuggestionProvider(brokerNodeList?.ToArray());
        }
        #endregion
        
        private BrokerSelectNode selectedBrokerNode;
        public BrokerSelectNode SelectedBrokerNode
        {
            get { return selectedBrokerNode; }
            set { SetProperty(ref selectedBrokerNode, value); }
        }

        private BroderPlatformSelectNode selectedBrokerPlatformNode;
        public BroderPlatformSelectNode SelectedBrokerPlatformNode
        {
            get { return selectedBrokerPlatformNode; }
            set { SetProperty(ref selectedBrokerPlatformNode, value); }
        }


        private BrokerAccessEntry selectedBrokerAccessEntry;
        public BrokerAccessEntry SelectedBrokerAccessEntry
        {
            get { return selectedBrokerAccessEntry; }
            set { SetProperty(ref selectedBrokerAccessEntry, value); }
        }

        private ICommand submitCmd;
        public ICommand SubmitCmd
        {
            get { return submitCmd; }
            set { SetProperty(ref submitCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }

        private AddFundAccount addAccount;
        public AddFundAccount AddAccount
        {
            get { return addAccount; }
            set { SetProperty(ref addAccount, value); }
        }
        
        private bool showAccountAliasAddRow;
        /// <summary>
        /// 是否显示昵称添加行
        /// </summary>
        public bool ShowAccountAliasAddRow
        {
            get { return showAccountAliasAddRow; }
            set { SetProperty(ref showAccountAliasAddRow, value); }
        }

    }

    public class BrokerSelectNodeListSuggestionProvider : ISuggestionProvider
    {
        private readonly IEnumerable<BrokerSelectNode> brokerNodeList;

        public BrokerSelectNodeListSuggestionProvider(IEnumerable<BrokerSelectNode> brokerNodeList)
        {
            this.brokerNodeList = brokerNodeList;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            filter = filter?.Trim();
            var list = brokerNodeList?.ToArray();
            if (string.IsNullOrEmpty(filter)) return list;
            return list?.Where(i => i.Node?.CnName?.Contains(filter)??false).ToArray();
        }
    }
}
