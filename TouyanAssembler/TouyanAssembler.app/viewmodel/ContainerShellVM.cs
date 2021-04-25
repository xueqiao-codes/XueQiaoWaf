using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using TouyanAssembler.app.view;
using XueQiaoFoundation.Shared.Model;

namespace TouyanAssembler.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContainerShellVM : ViewModel<ContainerShellWindow>
    {
        [ImportingConstructor]
        public ContainerShellVM(ContainerShellWindow view) : base(view)
        {
            TabNodes = new ObservableCollection<AppModuleTabNode>();
        }

        private ChromeWindowCaptionDataHolder windowCaptionDataHolder;
        public ChromeWindowCaptionDataHolder WindowCaptionDataHolder
        {
            get { return windowCaptionDataHolder; }
            set { SetProperty(ref windowCaptionDataHolder, value); }
        }

        public ObservableCollection<AppModuleTabNode> TabNodes { get; private set; }

        private AppModuleTabNode selectedTabNode;
        public AppModuleTabNode SelectedTabNode
        {
            get { return selectedTabNode; }
            set { SetProperty(ref selectedTabNode, value); }
        }

        private object tabNodeContentView;
        public object TabNodeContentView
        {
            get { return tabNodeContentView; }
            set { SetProperty(ref tabNodeContentView, value); }
        }

        private ICommand showAppInfoCmd;
        public ICommand ShowAppInfoCmd
        {
            get { return showAppInfoCmd; }
            set { SetProperty(ref showAppInfoCmd, value); }
        }

        public void Show()
        {
            ViewCore.Show();
        }

        public void Close()
        {
            ViewCore.Close();
        }

        public event CancelEventHandler Closing
        {
            add
            {
                ViewCore.Closing += value;
            }
            remove
            {
                ViewCore.Closing -= value;
            }
        }

        public event EventHandler Closed
        {
            add
            {
                ViewCore.Closed += value;
            }
            remove
            {
                ViewCore.Closed -= value;
            }
        }
    }
}
