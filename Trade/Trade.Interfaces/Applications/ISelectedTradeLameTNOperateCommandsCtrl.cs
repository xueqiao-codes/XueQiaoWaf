using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Applications
{
    /// <summary>
    /// 选中的成交瘸腿 Task Note 项 <see cref="XueQiaoFoundation.BusinessResources.DataModels.XQTradeLameTaskNote"/>操作 command 控制。
    /// 在引用方的生命周期结束前需要保持该实例，才能有效控制选中项的操作 command
    /// </summary>
    public interface ISelectedTradeLameTNOperateCommandsCtrl
    {
        /// <summary>
        /// 获取选中的成交瘸腿项操作 commands data model
        /// </summary>
        SelectedTradeLameTNOperateCommands SelectedTradeLameTNOptCommands { get; }
    }
}
