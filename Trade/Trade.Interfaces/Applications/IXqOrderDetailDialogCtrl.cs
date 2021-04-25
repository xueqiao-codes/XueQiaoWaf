using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Interfaces.Applications
{
    /// <summary>
    /// 雪橇订单详情弹窗控制器协议
    /// </summary>
    public interface IXqOrderDetailDialogCtrl
    {
        /// <summary>
        /// 弹窗的 owner
        /// </summary>
        object DialogOwner { get; set; }

        /// <summary>
        /// 雪橇订单 id
        /// </summary>
        string XqOrderId { get; set; }

        void Initialize();

        void Run();

        void Shutdown();
    }
}
