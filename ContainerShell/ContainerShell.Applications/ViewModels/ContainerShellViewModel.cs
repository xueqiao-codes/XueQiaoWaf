using ContainerShell.Applications.DataModels;
using ContainerShell.Applications.Views;
using ContainerShell.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using XueQiaoFoundation.Shared.Model;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContainerShellViewModel : ViewModel<IContainerShellView>
    {
        private readonly IUserMessageService userMessageService;

        [ImportingConstructor]
        public ContainerShellViewModel(IContainerShellView view, IUserMessageService userMessageService) : base(view)
        {
            this.userMessageService = userMessageService;

            TabNodes = new ObservableCollection<ContainerShellTabNodeItem>();
            InitializeItemCollectionDC = new InitializeItemCollectionDataContext();

            PropertyChangedEventManager.AddHandler(userMessageService, UserMessageServicePropChanged, "");
        }

        private void UserMessageServicePropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUserMessageService.UnreadMessageItemCount))
            {
                this.ExistUnreadUserMessages = (userMessageService.UnreadMessageItemCount > 0);
            }
        }

        private ChromeWindowCaptionDataHolder windowCaptionDataHolder;
        public ChromeWindowCaptionDataHolder WindowCaptionDataHolder
        {
            get { return windowCaptionDataHolder; }
            set { SetProperty(ref windowCaptionDataHolder, value); }
        }
        
        private ICommand showMoreFunctionCmd;
        public ICommand ShowMoreFunctionCmd
        {
            get { return showMoreFunctionCmd; }
            set { SetProperty(ref showMoreFunctionCmd, value); }
        }

        private ICommand showUserMessagePanelCmd;
        public ICommand ShowUserMessagePanelCmd
        {
            get { return showUserMessagePanelCmd; }
            set { SetProperty(ref showUserMessagePanelCmd, value); }
        }

        private bool existUnreadUserMessages;
        public bool ExistUnreadUserMessages
        {
            get { return existUnreadUserMessages; }
            set { SetProperty(ref existUnreadUserMessages, value); }
        }

        public ObservableCollection<ContainerShellTabNodeItem> TabNodes { get; private set; }

        private ContainerShellTabNodeItem selectedTabNode;
        public ContainerShellTabNodeItem SelectedTabNode
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

        public InitializeItemCollectionDataContext InitializeItemCollectionDC { get; private set; }

        private ICommand retryFailedInitializeCmd;
        public ICommand RetryFailedInitializeCmd
        {
            get { return retryFailedInitializeCmd; }
            set { SetProperty(ref retryFailedInitializeCmd, value); }
        }

        private ICommand skipFailedInitializeCmd;
        public ICommand SkipFailedInitializeCmd
        {
            get { return skipFailedInitializeCmd; }
            set { SetProperty(ref skipFailedInitializeCmd, value); }
        }

        private object appStatusBarView;
        /// <summary>
        /// 应用状态栏视图
        /// </summary>
        public object AppStatusBarView
        {
            get { return appStatusBarView; }
            set { SetProperty(ref appStatusBarView, value); }
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
        
        public object ShowMoreFunctionTriggerElement => ViewCore.ShowMoreFunctionTriggerElement;
        
    }

    public class InitializeItemCollectionDataContext : Model
    {
        public InitializeItemCollectionDataContext()
        {
            InitializeItems = new ObservableCollection<InitializeItem>();

            CollectionChangedEventManager.AddHandler(InitializeItems, InitializeItemCollectionChanged);
            InvalidateFailedInitializeItemCount();
            InvalidateCanSkipFailedInitializeItemCount();
        }
        
        public ObservableCollection<InitializeItem> InitializeItems { get; private set; }

        private int failedInitializeItemCount;
        /// <summary>
        /// 初始化失败项数目
        /// </summary>
        public int FailedInitializeItemCount
        {
            get { return failedInitializeItemCount; }
            private set { SetProperty(ref failedInitializeItemCount, value); }
        }

        private int canSkipFailedInitializeItemCount;
        /// <summary>
        /// 可跳过的初始化失败项数目
        /// </summary>
        public int CanSkipFailedInitializeItemCount
        {
            get { return canSkipFailedInitializeItemCount; }
            private set { SetProperty(ref canSkipFailedInitializeItemCount, value); }
        }

        private void InitializeItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.OfType<InitializeItem>().ToArray();
            var newItems = e.NewItems?.OfType<InitializeItem>().ToArray();
            if (oldItems?.Any() == true)
            {
                foreach (var item in oldItems)
                {
                    PropertyChangedEventManager.RemoveHandler(item, InitializeItemPropChanged, "");
                }
            }

            if (newItems?.Any() == true)
            {
                foreach (var item in newItems)
                {
                    PropertyChangedEventManager.RemoveHandler(item, InitializeItemPropChanged, "");
                    PropertyChangedEventManager.AddHandler(item, InitializeItemPropChanged, "");
                }
            }

            InvalidateFailedInitializeItemCount();
            InvalidateCanSkipFailedInitializeItemCount();
        }

        private void InitializeItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InitializeItem.IsInitializing) || e.PropertyName == nameof(InitializeItem.IsSuccess))
            {
                InvalidateFailedInitializeItemCount();
                InvalidateCanSkipFailedInitializeItemCount();
                return;
            }

            if (e.PropertyName == nameof(InitializeItem.CanSkipWhenFailed))
            {
                InvalidateCanSkipFailedInitializeItemCount();
                return;
            }
        }

        private void InvalidateFailedInitializeItemCount()
        {
            FailedInitializeItemCount = InitializeItems.Count(i => i.IsInitializing == false && i.IsSuccess == false);
        }

        private void InvalidateCanSkipFailedInitializeItemCount()
        {
            CanSkipFailedInitializeItemCount = InitializeItems.Count(i => i.IsInitializing == false && i.IsSuccess == false && i.CanSkipWhenFailed == true);
        }
    }
}
