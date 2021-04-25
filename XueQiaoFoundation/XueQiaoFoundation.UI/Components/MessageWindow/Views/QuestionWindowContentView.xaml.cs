using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XueQiaoFoundation.UI.Components.MessageWindow.ViewModels;

namespace XueQiaoFoundation.UI.Components.MessageWindow.Views
{
    /// <summary>
    /// QuestionWindowContentView.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class QuestionWindowContentView : IView
    {
        private readonly Lazy<QuestionWindowContentVM> contentVMLazy;

        public QuestionWindowContentView()
        {
            InitializeComponent();
            contentVMLazy = new Lazy<QuestionWindowContentVM>(() => ViewHelper.GetViewModel<QuestionWindowContentVM>(this));
        }

        private void PositiveButton_Click(object sender, RoutedEventArgs e)
        {
            //var displayInWindow = Window.GetWindow(this);
            //if (displayInWindow != null)
            //{
            //    contentVMLazy.Value.PublishDialogResultChanged(true);
            //    displayInWindow.DialogResult = true;
            //}
            contentVMLazy.Value?.PositiveButtonClickAction?.Invoke();
        }

        private void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            //var displayInWindow = Window.GetWindow(this);
            //if (displayInWindow != null)
            //{
            //    contentVMLazy.Value.PublishDialogResultChanged(false);
            //    displayInWindow.DialogResult = false;
            //}
            contentVMLazy.Value?.NegativeButtonClickAction?.Invoke();
        }

        public RoutedEventHandler DialogCloseMenuButtonManualClicked
        {
            get { return this._dialogCloseMenuButtonManualClicked; }
        }

        private void _dialogCloseMenuButtonManualClicked(object sender, RoutedEventArgs e)
        {
            //var displayInWindow = Window.GetWindow(this);
            //if (displayInWindow != null)
            //{
            //    e.Handled = true;
            //    contentVMLazy.Value.PublishDialogResultChanged(null);
            //    displayInWindow.DialogResult = null;
            //    displayInWindow.Close();
            //}
            contentVMLazy.Value?.CornerCloseButtonClickAction.Invoke();
        }
    }
}
