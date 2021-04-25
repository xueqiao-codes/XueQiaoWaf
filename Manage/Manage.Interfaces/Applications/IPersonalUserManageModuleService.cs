using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Interfaces.Applications
{
    public interface IPersonalUserManageModuleService
    {
        /// <summary>
        /// 获取`个人用户管理`模块的根视图
        /// </summary>
        /// <param name="embedInWindowCaptionDataHolderGetter">视图嵌入到窗体头部时的数据 holder 的获取方法</param>
        /// <param name="showAction">切换到显示该 tab 时的回调</param>
        /// <param name="closeAction">切换到关闭该 tab 的回调</param>
        /// <returns></returns>
        object GetPersonalUserManageModuleRootView(Func<ChromeWindowCaptionDataHolder> embedInWindowCaptionDataHolderGetter,
            out Action showAction, out Action closeAction);
    }
}
