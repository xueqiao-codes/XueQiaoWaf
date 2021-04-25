using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Input;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class SelectedComposesOperateCommands : Model
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

        private ICommand renameCmd;
        /// <summary>
        /// 为选中项重命名 command
        /// </summary>
        public ICommand RenameCmd
        {
            get { return renameCmd; }
            set { SetProperty(ref renameCmd, value); }
        }

        private ICommand editPrecisionNumberCmd;
        /// <summary>
        /// 编辑小数位数 command
        /// </summary>
        public ICommand EditPrecisionNumberCmd
        {
            get { return editPrecisionNumberCmd; }
            set { SetProperty(ref editPrecisionNumberCmd, value); }
        }

        private ICommand createNewFromCopyCmd;
        /// <summary>
        /// 复制成立新组合 command
        /// </summary>
        public ICommand CreateNewFromCopyCmd
        {
            get { return createNewFromCopyCmd; }
            set { SetProperty(ref createNewFromCopyCmd, value); }
        }
    }
}
