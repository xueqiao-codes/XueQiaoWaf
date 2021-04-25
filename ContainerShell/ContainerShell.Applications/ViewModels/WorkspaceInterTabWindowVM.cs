using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class WorkspaceInterTabWindowVM : ViewModel<IWorkspaceInterTabWindow>
    {
        [ImportingConstructor]
        public WorkspaceInterTabWindowVM(IWorkspaceInterTabWindow view) : base(view)
        {
            view.Loaded += ViewLoaded;
            view.Closing += ViewClosing;
            view.Closed += ViewClosed;
        }

        private object workspaceTabControlView;
        public object WorkspaceTabControlView
        {
            get { return workspaceTabControlView; }
            set { SetProperty(ref workspaceTabControlView, value); }
        }
        
        public object ConainerWindow => ViewCore.ConainerWindow;

        private TabWorkspaceWindow workspaceWindow;
        public TabWorkspaceWindow WorkspaceWindow
        {
            get { return workspaceWindow; }
            set { SetProperty(ref workspaceWindow, value); }
        }

        private ChromeWindowCaptionDataHolder windowCaptionDataHolder;
        public ChromeWindowCaptionDataHolder WindowCaptionDataHolder
        {
            get { return windowCaptionDataHolder; }
            set { SetProperty(ref windowCaptionDataHolder, value); }
        }

        public void ShowWindow()
        {
            ViewCore.Show();
        }

        public void CloseWindow(bool force)
        {
            if (force)
            {
                ViewCore.Closing -= ViewClosing;
                ViewCore.Closed -= ViewClosed;

                ViewCore.Close();

                ViewCore.Closing += ViewClosing;
                ViewCore.Closed += ViewClosed;
            }
            else
            {
                ViewCore.Close();
            }
        }

        public event Action<WorkspaceInterTabWindowVM, RoutedEventArgs> Loaded;

        public event Action<WorkspaceInterTabWindowVM, CancelEventArgs> Closing;

        public event Action<WorkspaceInterTabWindowVM, EventArgs> Closed;

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            Loaded?.Invoke(this, e);
        }

        private void ViewClosing(object sender, CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        private void ViewClosed(object sender, EventArgs e)
        {
            Closed?.Invoke(this, e);
        }
    }
}
