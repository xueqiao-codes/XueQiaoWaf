using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using System.Windows.Input;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 选中的成交瘸腿 Task Note <see cref="XQTradeLameTaskNote"/> 操作 command 列表 datamodel
    /// </summary>
    public class SelectedTradeLameTNOperateCommands : Model
    {
        private ICommand tradeLameTNSelectionChangedCmd;
        /// <summary>
        /// 列表选中变化 command
        /// </summary>
        public ICommand TradeLameTNSelectionChangedCmd
        {
            get { return tradeLameTNSelectionChangedCmd; }
            set { SetProperty(ref tradeLameTNSelectionChangedCmd, value); }
        }
        
        private ICommand deleteSelectedTNsCmd;
        /// <summary>
        /// 删除选中的 Task Note command
        /// </summary>
        public ICommand DeleteSelectedTNsCmd
        {
            get { return deleteSelectedTNsCmd; }
            set { SetProperty(ref deleteSelectedTNsCmd, value); }
        }

    }
}
