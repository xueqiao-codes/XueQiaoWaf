using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Input;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    public class SelectedOrdersOperateCommands : Model
    {
        private ICommand orderItemsSelectionChangedCmd;
        /// <summary>
        /// 选中变化 command
        /// </summary>
        public ICommand OrderItemsSelectionChangedCmd
        {
            get { return orderItemsSelectionChangedCmd; }
            set { SetProperty(ref orderItemsSelectionChangedCmd, value); }
        }

        private ICommand revokeSelectedOrdersCmd;
        /// <summary>
        /// 撤单 command 
        /// </summary>
        public ICommand RevokeSelectedOrdersCmd
        {
            get { return revokeSelectedOrdersCmd; }
            set { SetProperty(ref revokeSelectedOrdersCmd, value); }
        }

        private ICommand suspendSelectedOrdersCmd;
        /// <summary>
        /// 暂停 command
        /// </summary>
        public ICommand SuspendSelectedOrdersCmd
        {
            get { return suspendSelectedOrdersCmd; }
            set { SetProperty(ref suspendSelectedOrdersCmd, value); }
        }

        private ICommand resumeSelectedOrdersCmd;
        /// <summary>
        /// 继续 command
        /// </summary>
        public ICommand ResumeSelectedOrdersCmd
        {
            get { return resumeSelectedOrdersCmd; }
            set { SetProperty(ref resumeSelectedOrdersCmd, value); }
        }

        private ICommand strongChaseSelectedOrdersCmd;
        /// <summary>
        /// 强追 command
        /// </summary>
        public ICommand StrongChaseSelectedOrdersCmd
        {
            get { return strongChaseSelectedOrdersCmd; }
            set { SetProperty(ref strongChaseSelectedOrdersCmd, value); }
        }

    }
}
