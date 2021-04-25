using Manage.Applications.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manage.Presentations.Views
{
    /// <summary>
    /// WinzardProcessBarView.xaml 的交互逻辑
    /// </summary>
    public partial class WinzardStepView
    {
        public WinzardStepView()
        {
            InitializeComponent();
        }

        private void View_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is WizardStepItem dataContext)
            {
                var items = new List<Int32>();
                for (var i = 0; i < dataContext.StepCount; i++)
                {
                    items.Add(i);
                }
                this.WizardStepItemsControl.ItemsSource = items;
            }
        }
    }
}
