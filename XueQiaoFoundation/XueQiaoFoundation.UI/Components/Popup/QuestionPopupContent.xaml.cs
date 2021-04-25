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

namespace XueQiaoFoundation.UI.Components.Popup
{
    /// <summary>
    /// QuestionPopupContent.xaml 的交互逻辑
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class QuestionPopupContent : IView
    {
        public QuestionPopupContent()
        {
            InitializeComponent();
        }

        private string questionMessage;
        /// <summary>
        /// Get or set question message
        /// </summary>
        public string QuestionMessage
        {
            get { return questionMessage; }
            set
            {
                if (Equals(questionMessage, value)) return;
                questionMessage = value;
                this.QuestionMessageText.Text = questionMessage;
            }
        }

        /// <summary>
        /// Get or set visible of PositiveButton
        /// </summary>
        public bool PositiveButtonVisible
        {
            get { return PositiveButton.Visibility == Visibility.Visible; }
            set
            {
                PositiveButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Get or set visible of NegativeButton
        /// </summary>
        public bool NegativeButtonVisible
        {
            get { return NegativeButton.Visibility == Visibility.Visible; }
            set
            {
                NegativeButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private string positiveButtonTitle;
        /// <summary>
        /// Get or set title of PositiveButton
        /// </summary>
        public string PositiveButtonTitle
        {
            get { return positiveButtonTitle; }
            set
            {
                if (Equals(positiveButtonTitle, value)) return;
                positiveButtonTitle = value;
                this.PositiveButton.Content = positiveButtonTitle;
            }
        }

        private string negativeButtonTitle;
        /// <summary>
        /// Get or set title of NegativeButton
        /// </summary>
        public string NegativeButtonTitle
        {
            get { return negativeButtonTitle; }
            set
            {
                if (Equals(negativeButtonTitle, value)) return;
                negativeButtonTitle = value;
                this.NegativeButton.Content = negativeButtonTitle;
            }
        }

        /// <summary>
        /// Add or remove clicked event of PositiveButton
        /// </summary>
        public event RoutedEventHandler PositiveButtonClicked
        {
            add { this.PositiveButton.Click += value; }
            remove { this.PositiveButton.Click -= value; }
        }

        /// <summary>
        /// Add or remove clicked event of NegativeButton
        /// </summary>
        public event RoutedEventHandler NegativeButtonClicked
        {
            add { this.NegativeButton.Click += value; }
            remove { this.NegativeButton.Click -= value; }
        }
    }
}
