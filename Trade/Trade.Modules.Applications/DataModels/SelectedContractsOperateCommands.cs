using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Input;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class SelectedContractsOperateCommands : Model
    {
        private ICommand itemsSelectionChangedCmd;
        /// <summary>
        /// 选中项变化 command。
        /// </summary>
        public ICommand ItemsSelectionChangedCmd
        {
            get { return itemsSelectionChangedCmd; }
            set { SetProperty(ref itemsSelectionChangedCmd, value); }
        }

        private ICommand showContractInfoCmd;
        /// <summary>
        /// 查看选中项合约详情
        /// </summary>
        public ICommand ShowContractInfoCmd
        {
            get { return showContractInfoCmd; }
            set { SetProperty(ref showContractInfoCmd, value); }
        }

        private ICommand subscribeQuotationCmd;
        /// <summary>
        /// 为选中项退订行情
        /// </summary>
        public ICommand SubscribeQuotationCmd
        {
            get { return subscribeQuotationCmd; }
            set { SetProperty(ref subscribeQuotationCmd, value); }
        }

        private ICommand unsubscribeQuotationCmd;
        /// <summary>
        /// 为选中项退订行情
        /// </summary>
        public ICommand UnsubscribeQuotationCmd
        {
            get { return unsubscribeQuotationCmd; }
            set { SetProperty(ref unsubscribeQuotationCmd, value); }
        }

        private ICommand removeItemCmd;
        /// <summary>
        /// 删除选中项 command。param type<see cref="SubscribeDataGroup"/>
        /// </summary>
        public ICommand RemoveItemCmd
        {
            get { return removeItemCmd; }
            set { SetProperty(ref removeItemCmd, value); }
        }

        private ICommand addToGroupCmd;
        /// <summary>
        /// 添加选中项至分组 command。param type<see cref="SubscribeDataGroup"/>
        /// </summary>
        public ICommand AddToGroupCmd
        {
            get { return addToGroupCmd; }
            set { SetProperty(ref addToGroupCmd, value); }
        }

        private ICommand removeFromGroupCmd;
        /// <summary>
        /// 从分组中删除选中项 command。param type <see cref="SubscribeDataGroup"/>
        /// </summary>
        public ICommand RemoveFromGroupCmd
        {
            get { return removeFromGroupCmd; }
            set { SetProperty(ref removeFromGroupCmd, value); }
        }

        private ICommand openPlaceOrderComponentCmd;
        /// <summary>
        /// 为选中的项打开下单组件
        /// </summary>
        public ICommand OpenPlaceOrderComponentCmd
        {
            get { return openPlaceOrderComponentCmd; }
            set { SetProperty(ref openPlaceOrderComponentCmd, value); }
        }

    }
}
