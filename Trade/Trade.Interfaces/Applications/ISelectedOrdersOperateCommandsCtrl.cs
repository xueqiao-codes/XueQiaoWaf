using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;

namespace XueQiaoWaf.Trade.Interfaces.Applications
{
    /// <summary>
    /// 选中的订单项操作 command 控制。在引用方的生命周期结束前需要保持该实例，才能有效控制选中项的操作 command
    /// </summary>
    public interface ISelectedOrdersOperateCommandsCtrl
    {
        /// <summary>
        /// 需要弹出窗口时的 owner 获取方法
        /// </summary>
        Func<object> WindowOwnerGetter { get; set; }

        /// <summary>
        /// 获取选中订单操作 commands model
        /// </summary>
        SelectedOrdersOperateCommands SelectedOrdersOptCommands { get; }

        void Initialize();

        void Shutdown();
    }
}
