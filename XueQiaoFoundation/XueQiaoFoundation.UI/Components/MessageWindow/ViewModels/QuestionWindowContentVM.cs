using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Components.MessageWindow.Views;

namespace XueQiaoFoundation.UI.Components.MessageWindow.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class QuestionWindowContentVM : ViewModel<QuestionWindowContentView>
    {
        [ImportingConstructor]
        public QuestionWindowContentVM(QuestionWindowContentView view) : base(view)
        {
            CaptionHeightHolder = new MessageWindowCaptionHeightHolder();
        }

        public MessageWindowCaptionHeightHolder CaptionHeightHolder { get; private set; }

        private object dialogTitle;
        public object DialogTitle
        {
            get { return dialogTitle; }
            set { SetProperty(ref dialogTitle, value); }
        }
        
        private object message;
        public object Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
        
        private bool isHideCloseDialogMenuButton;
        public bool IsHideCloseDialogMenuButton
        {
            get { return isHideCloseDialogMenuButton; }
            set { SetProperty(ref isHideCloseDialogMenuButton, value); }
        }

        private string positiveButtonTitle;
        public string PositiveButtonTitle
        {
            get { return positiveButtonTitle; }
            set { SetProperty(ref positiveButtonTitle, value); }
        }

        private string negativeButtonTitle;
        public string NegativeButtonTitle
        {
            get { return negativeButtonTitle; }
            set { SetProperty(ref negativeButtonTitle, value); }
        }

        /// <summary>
        /// positive button 点击 action
        /// </summary>
        public Action PositiveButtonClickAction { get; set; }

        /// <summary>
        /// negative button 点击 action
        /// </summary>
        public Action NegativeButtonClickAction { get; set; }

        /// <summary>
        /// corner close button 点击 action
        /// </summary>
        public Action CornerCloseButtonClickAction { get; set; }

        /// <summary>
        /// 弹窗处理结果。null 表示取消， true 表示 positive button 点击，false 表示 negative button 点击
        /// </summary>
        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            private set { this.dialogResult = value; }
        }

        /// <summary>
        /// 发布关闭的事件
        /// </summary>
        /// <param name="closingItem">要关闭的项</param>
        /// <param name="e">可取消的句柄</param>
        public void PublishDialogResultChanged(bool? dialogResultNewValue)
        {
            this.DialogResult = dialogResultNewValue;
        }
    }
}
