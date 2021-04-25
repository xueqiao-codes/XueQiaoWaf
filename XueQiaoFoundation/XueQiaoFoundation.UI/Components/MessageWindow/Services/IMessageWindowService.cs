using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.UI.Components.MessageWindow.Services
{
    public interface IMessageWindowService
    {
        /// <summary>
        /// 显示简单信息模态弹窗
        /// </summary>
        void ShowMessageDialog(object owner, Point? windowLocation, Size? windowSize,
            string dialogTitle, object message, string buttonTitle = null);

        /// <summary>
        /// 创建简单的信息窗口
        /// </summary>
        /// <returns>信息窗口</returns>
        IMessageWindow CreateMessageWindow(object owner, Point? windowLocation, Size? windowSize,
            string windowTitle, object message, string buttonTitle = null);

        /// <summary>
        /// 显示问题式的模态弹窗
        /// </summary>
        /// <returns>null 表示取消， true 表示 positive button 点击，false 表示 negative button 点击</returns>
        bool? ShowQuestionDialog(object owner, Point? windowLocation, Size? windowSize,
            string dialogTitle, object message, bool showCloseDialogMenuButton,
            string positiveButtonTitle = null, string negativeButtonTitle = null);


        /// <summary>
        /// 创建问题提示窗口
        /// </summary>
        /// <param name="questionResultHandler">问题结果处理。true 为 Position button 点击给结果，false 为Negative button 点击结果，null 为 Close button 点击结果。</param>
        /// <returns>信息窗口</returns>
        IMessageWindow CreateQuestionWindow(object owner, Point? windowLocation, Size? windowSize,
            string dialogTitle, object message, bool showCloseDialogMenuButton,
            string positiveButtonTitle = null, string negativeButtonTitle = null, Action<IMessageWindow, bool?> questionResultHandler = null);

        /// <summary>
        /// 创建自定义内容部分视图的window对象
        /// </summary>
        IMessageWindow CreateContentCustomWindow(object owner, Point? windowLocation, Size? windowSize,
            bool windowShowInTaskbar, bool windowCanResize,
            bool showCloseWindowMenuButton, object windowTitle, object contentCustomView);

        /// <summary>
        /// 创建自定义全部视图的window对象
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="windowLocation"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowShowInTaskbar"></param>
        /// <param name="windowCanResize"></param>
        /// <param name="windowAbsoluteCustomView"></param>
        /// <param name="windowCaptionHeightHolder">用于控制窗口高度的对象</param>
        /// <returns></returns>
        IMessageWindow CreateCompleteCustomWindow(object owner, Point? windowLocation, Size? windowSize,
                    bool windowShowInTaskbar,
                    bool windowCanResize,
                    object windowAbsoluteCustomView,
                    MessageWindowCaptionHeightHolder windowCaptionHeightHolder);

    }
}
