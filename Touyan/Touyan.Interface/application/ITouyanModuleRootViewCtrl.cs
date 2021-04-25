using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace Touyan.Interface.application
{
    /// <summary>
    /// 投研模块根视图控制器 协议
    /// </summary>
    public interface ITouyanModuleRootViewCtrl
    {
        /// <summary>
        /// 当嵌入到窗体头部时，窗体头部数据的 holder
        /// </summary>
        ChromeWindowCaptionDataHolder EmbedInWindowCaptionDataHolder { get; set; }

        /// <summary>
        /// 内容视图
        /// </summary>
        object ContentView { get; }

        void Initialize();
        void Run();
        void Shutdown();
    }
}
