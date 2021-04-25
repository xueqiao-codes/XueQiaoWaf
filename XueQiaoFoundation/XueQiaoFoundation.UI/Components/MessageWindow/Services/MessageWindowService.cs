using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using XueQiaoFoundation.UI.Components.MessageWindow.ViewModels;

namespace XueQiaoFoundation.UI.Components.MessageWindow.Services
{
    [Export(typeof(IMessageWindowService)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class MessageWindowService : IMessageWindowService
    {
        private readonly ExportFactory<MessageLayoutWindowVM> messageLayoutWindowVMFactory;
        private readonly ExportFactory<SimpleMessageWindowContentVM> simpleMsgWindowContentVMFactory;
        private readonly ExportFactory<QuestionWindowContentVM> questionWindowContentVMFactory;
        private readonly ExportFactory<ContentCustomWindowContentVM> contentCustomWindowContentVMfactory;

        [ImportingConstructor]
        public MessageWindowService(
            ExportFactory<MessageLayoutWindowVM> messageLayoutWindowVMFactory,
            ExportFactory<SimpleMessageWindowContentVM> simpleMsgWindowContentVMFactory,
            ExportFactory<QuestionWindowContentVM> questionWindowContentVMFactory,
            ExportFactory<ContentCustomWindowContentVM> contentCustomWindowContentVMfactory)
        {
            this.messageLayoutWindowVMFactory = messageLayoutWindowVMFactory;
            this.simpleMsgWindowContentVMFactory = simpleMsgWindowContentVMFactory;
            this.questionWindowContentVMFactory = questionWindowContentVMFactory;
            this.contentCustomWindowContentVMfactory = contentCustomWindowContentVMfactory;
        }
        
        public void ShowMessageDialog(object owner, Point? windowLocation, Size? windowSize,
            string dialogTitle, object message, string buttonTitle = null)
        {
            var messageWindow = CreateMessageWindow(owner, windowLocation, windowSize, dialogTitle, message, buttonTitle);
            messageWindow.ShowDialog();
        }

        
        public IMessageWindow CreateMessageWindow(object owner, Point? windowLocation, Size? windowSize,
            string windowTitle, object message, string buttonTitle = null)
        {
            var simpleMsgContentVM = simpleMsgWindowContentVMFactory.CreateExport().Value;
            simpleMsgContentVM.DialogTitle = windowTitle;
            simpleMsgContentVM.Message = message;
            simpleMsgContentVM.ButtonTitle = buttonTitle;

            var layoutVM = messageLayoutWindowVMFactory.CreateExport().Value;
            layoutVM.ContentView = simpleMsgContentVM.View;
            layoutVM.CaptionHeightHolder = simpleMsgContentVM.CaptionHeightHolder;
            layoutVM.WindowCanResize = false;
            layoutVM.WindowShowInTaskbar = false;

            ConfigMessageLayoutWindowVM(layoutVM, owner, windowLocation, windowSize, 400, null);

            simpleMsgContentVM.ButtonClickAction = () => layoutVM.Close();

            return layoutVM;
        }

        public bool? ShowQuestionDialog(object owner, Point? windowLocation, Size? windowSize, 
            string dialogTitle, object message, bool showCloseDialogMenuButton,
            string positiveButtonTitle = null, string negativeButtonTitle = null)
        {
            var resultHolder = new List<bool?>();
            var messageWindow = CreateQuestionWindow(owner, windowLocation, windowSize, dialogTitle, message, showCloseDialogMenuButton,
                positiveButtonTitle, negativeButtonTitle, (_win, _qr) => 
                {
                    resultHolder.Add(_qr);
                    _win?.Close();
                });
            messageWindow.ShowDialog();
            return resultHolder.LastOrDefault() ?? null;
        }

        public IMessageWindow CreateQuestionWindow(object owner, Point? windowLocation, Size? windowSize,
            string dialogTitle, object message, bool showCloseDialogMenuButton,
            string positiveButtonTitle = null, string negativeButtonTitle = null, Action<IMessageWindow, bool?> questionResultHandler = null)
        {
            var questionWindowContentVM = questionWindowContentVMFactory.CreateExport().Value;
            questionWindowContentVM.DialogTitle = dialogTitle;
            questionWindowContentVM.Message = message;
            questionWindowContentVM.IsHideCloseDialogMenuButton = !showCloseDialogMenuButton;
            questionWindowContentVM.PositiveButtonTitle = positiveButtonTitle;
            questionWindowContentVM.NegativeButtonTitle = negativeButtonTitle;

            var layoutVM = messageLayoutWindowVMFactory.CreateExport().Value;
            layoutVM.ContentView = questionWindowContentVM.View;
            layoutVM.CaptionHeightHolder = questionWindowContentVM.CaptionHeightHolder;
            layoutVM.WindowCanResize = false;
            layoutVM.WindowShowInTaskbar = false;

            ConfigMessageLayoutWindowVM(layoutVM, owner, windowLocation, windowSize, 400, null);

            questionWindowContentVM.PositiveButtonClickAction = () => 
            {
                questionResultHandler?.Invoke(layoutVM, true);
            };
            questionWindowContentVM.NegativeButtonClickAction = () =>
            {
                questionResultHandler?.Invoke(layoutVM, false);
            };
            questionWindowContentVM.CornerCloseButtonClickAction = () =>
            {
                questionResultHandler?.Invoke(layoutVM, null);
            };

            return layoutVM;
        }


        public IMessageWindow CreateContentCustomWindow(object owner, Point? windowLocation, Size? windowSize,
            bool windowShowInTaskbar, bool windowCanResize, 
            bool showCloseWindowMenuButton, object windowTitle, object contentCustomView)
        {
            var dialogContentVM = contentCustomWindowContentVMfactory.CreateExport().Value;
            dialogContentVM.DialogTitle = windowTitle;
            dialogContentVM.ContentCustomView = contentCustomView;
            dialogContentVM.IsHideCloseDialogMenuButton = !showCloseWindowMenuButton;

            var layoutVM = messageLayoutWindowVMFactory.CreateExport().Value;
            layoutVM.ContentView = dialogContentVM.View;
            layoutVM.CaptionHeightHolder = dialogContentVM.CaptionHeightHolder;
            layoutVM.WindowCanResize = windowCanResize;
            layoutVM.WindowShowInTaskbar = windowShowInTaskbar;
            
            ConfigMessageLayoutWindowVM(layoutVM, owner, windowLocation, windowSize, null, null);

            return layoutVM;
        }

        public IMessageWindow CreateCompleteCustomWindow(object owner, Point? windowLocation, Size? windowSize,
                    bool windowShowInTaskbar,
                    bool windowCanResize,
                    object windowAbsoluteCustomView,
                    MessageWindowCaptionHeightHolder windowCaptionHeightHolder)
        {
            var layoutVM = messageLayoutWindowVMFactory.CreateExport().Value;
            layoutVM.ContentView = windowAbsoluteCustomView;
            layoutVM.CaptionHeightHolder = windowCaptionHeightHolder;
            layoutVM.WindowCanResize = windowCanResize;
            layoutVM.WindowShowInTaskbar = windowShowInTaskbar;

            ConfigMessageLayoutWindowVM(layoutVM, owner, windowLocation, windowSize, null, null);

            return layoutVM;
        }

        private static void ConfigMessageLayoutWindowVM(MessageLayoutWindowVM layoutVM,
            object owner, Point? windowLocation, Size? windowSize, double? maxWidth, double? maxHeight)
        {
            if (layoutVM == null) return;

            layoutVM.WindowOwner = owner;
            if (windowLocation != null)
            {
                layoutVM.WindowLocation = windowLocation.Value;
            }
            else
            {
                layoutVM.WindowStartupLocation = owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
            }

            if (windowSize != null)
            {
                layoutVM.WindowSize = windowSize.Value;
                layoutVM.WindowSizeToContent = SizeToContent.Manual;
            }
            else
            {
                layoutVM.WindowSizeToContent = SizeToContent.WidthAndHeight;
            }

            if (maxWidth != null)
            {
                layoutVM.WindowMaxWidth = maxWidth.Value;
            }

            if (maxHeight != null)
            {
                layoutVM.WindowMaxHeight = maxHeight.Value;
            }
        }
    }
}
