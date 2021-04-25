using ContainerShell.Applications.ViewModels;
using ContainerShell.Applications.Views;
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

namespace ContainerShell.Presentations.Views
{
    /// <summary>
    /// OrderOccurExceptionNDP1.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IOrderAmbiguousNDP)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class OrderAmbiguousNDP : IOrderAmbiguousNDP
    {
        private Lazy<OrderAmbiguousNDPVM> ndpVM;

        public OrderAmbiguousNDP()
        {
            InitializeComponent();
            ndpVM = new Lazy<OrderAmbiguousNDPVM>(() => ViewHelper.GetViewModel<OrderAmbiguousNDPVM>(this));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            var notification = ndpVM.Value?.ToastNotification;
            if (notification != null)
                notification.CanClose = false;

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            var notification = ndpVM.Value?.ToastNotification;
            if (notification != null)
                notification.CanClose = true;

            base.OnMouseLeave(e);
        }
    }
}
