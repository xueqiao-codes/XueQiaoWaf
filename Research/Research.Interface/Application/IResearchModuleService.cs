using Research.Interface.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Research.Interface.Application
{
    public interface IResearchModuleService
    {
        /// <summary>
        /// 投研模块工作空间数据根
        /// </summary>
        ResearchWorkspaceDataRoot ResearchWorkspaceDataRoot { get; }
        
        /// <summary>
        /// 获取投研模块的根视图
        /// </summary>
        /// <param name="showAction">切换到显示该 tab 时的回调</param>
        /// <param name="closeAction">切换到关闭该 tab 的回调</param>
        /// <returns></returns>
        object GetResearchModuleRootView(out Action showAction, out Action closeAction);

        /// <summary>
        /// 生成投研 workspace unique key
        /// </summary>
        string GenerateResearchWorkspaceKey();
    }
}
