using business_foundation_lib.performance_monitor;
using lib.configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Models;

namespace AppAssembler.Interfaces.Applications
{
    public delegate void ThemeApplied(string themeName);

    public delegate void LanguageApplied(XqAppLanguages language);

    public delegate void AppShutdown();

    public interface IAppAssemblerService
    {
        /// <summary>
        /// 是否正在显示最初始界面
        /// </summary>
        bool IsShowingStartupWindow { get; }

        /// <summary>
        /// 显示最初始界面
        /// </summary>
        void ShowStartupUI();

        /// <summary>
        /// 应用主题
        /// </summary>
        /// <param name="themeName"></param>
        void ApplyTheme(string themeName);

        /// <summary>
        /// 主题已应用的事件
        /// </summary>
        event ThemeApplied ThemeApplied;
        
        /// <summary>
        /// 应用语言
        /// </summary>
        /// <param name="language"></param>
        void ApplyLanguage(XqAppLanguages language);

        /// <summary>
        /// 语言已应用的事件
        /// </summary>
        event LanguageApplied LanguageApplied;

        /// <summary>
        /// 应用偏好设置管理器
        /// </summary>
        ConfigurationManager<XqAppPreference> PreferenceManager { get; }

        /// <summary>
        /// 关闭应用
        /// </summary>
        void ShutdownApplication();

        /// <summary>
        /// 应用关闭事件
        /// </summary>
        event AppShutdown AppShutdown;

        /// <summary>
        /// 应用的性能数据变化的事件
        /// </summary>
        event CurrentProcessPerformanceDataChanged AppPerformanceDataChanged;
    }
}
