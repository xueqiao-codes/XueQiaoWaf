using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace TouyanAssembler.Interface.application
{
    /// <summary>
    /// 主界面模块协议
    /// </summary>
    public interface IContainerShellModuleService
    {
        /// <summary>
        /// 显示主界面窗口
        /// </summary>
        void ShowShellWindow();

        /// <summary>
        /// 主界面窗口关闭中事件
        /// </summary>
        event CancelEventHandler ShellWindowClosing;

        /// <summary>
        /// 主界面窗口关闭事件
        /// </summary>
        event EventHandler ShellWindowClosed;

        #region (获取模块的根视图。如果根视图为 null，则保持原来的显示视图不变。)
        
        /// <summary>
        /// 获取`交易`模块的根视图。
        /// </summary>
        /// <param name="embedInWindowCaptionDataHolderGetter"></param>
        /// <param name="moduleRootView"></param>
        /// <param name="showAction"></param>
        /// <param name="closeAction"></param>
        void GetTradeModuleRootView(Func<ChromeWindowCaptionDataHolder> embedInWindowCaptionDataHolderGetter,
            out object moduleRootView, out Action showAction, out Action closeAction);

        /// <summary>
        /// 获取`投研`模块的根视图。
        /// </summary>
        /// <param name="embedInWindowCaptionDataHolderGetter"></param>
        /// <param name="moduleRootView"></param>
        /// <param name="showAction"></param>
        /// <param name="closeAction"></param>
        void GetTouyanModuleRootView(Func<ChromeWindowCaptionDataHolder> embedInWindowCaptionDataHolderGetter,
            out object moduleRootView, out Action showAction, out Action closeAction);

        #endregion
    }
}
