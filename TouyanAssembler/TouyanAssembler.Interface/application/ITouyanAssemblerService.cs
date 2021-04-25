using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouyanAssembler.Interface.application
{
    public delegate void AppShutdown();

    public interface ITouyanAssemblerService
    {
        /// <summary>
        /// 重置到初始界面
        /// </summary>
        void ResetToStartupUI();

        /// <summary>
        /// 关闭应用
        /// </summary>
        void ShutdownApplication();

        /// <summary>
        /// 应用关闭事件
        /// </summary>
        event AppShutdown AppShutdown;
    }
}
