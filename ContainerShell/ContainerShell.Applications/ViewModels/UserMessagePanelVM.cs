using ContainerShell.Applications.Views;
using ContainerShell.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using xueqiao.mailbox.user.message.thriftapi;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserMessagePanelVM : ViewModel<IUserMessagePanelView>
    {
        private readonly IUserMessageService userMessageService;
        private readonly ListCollectionView messageListCollectionView;

        [ImportingConstructor]
        public UserMessagePanelVM(IUserMessagePanelView view,
            IUserMessageService userMessageService) : base(view)
        {
            this.userMessageService = userMessageService;
            WindowCaptionHeightHolder = new MessageWindowCaptionHeightHolder();

            var syncMsgs = new SynchronizingCollection<UserMessage, UserMessage>(userMessageService.MessageItems, i => i);
            this.messageListCollectionView = CollectionViewSource.GetDefaultView(syncMsgs) as ListCollectionView;
            InitialConfigMessageListCollectionView();

            this.SelectedMessageItem = this.messageListCollectionView?.OfType<UserMessage>()?.FirstOrDefault();
        }

        public MessageWindowCaptionHeightHolder WindowCaptionHeightHolder { get; private set; }
        
        /// <summary>
        /// 关闭窗口按钮处理
        /// </summary>
        private RoutedEventHandler closeMenuButtonClickHandler;
        public RoutedEventHandler CloseMenuButtonClickHandler
        {
            get { return closeMenuButtonClickHandler; }
            set { SetProperty(ref closeMenuButtonClickHandler, value); }
        }
        
        public ICollectionView MessageCollectionView
        {
            get { return messageListCollectionView; }
        }

        private UserMessage selectedMessageItem;
        public UserMessage SelectedMessageItem
        {
            get { return selectedMessageItem; }
            set { SetProperty(ref selectedMessageItem, value); }
        }

        private object selectedMessageContentView;
        public object SelectedMessageContentView
        {
            get { return selectedMessageContentView; }
            set { SetProperty(ref selectedMessageContentView, value); }
        }

        private ICommand loadMoreItemsCmd;
        public ICommand LoadMoreItemsCmd
        {
            get { return loadMoreItemsCmd; }
            set { SetProperty(ref loadMoreItemsCmd, value); }
        }

        private ICommand refreshItemsCmd;
        public ICommand RefreshItemsCmd
        {
            get { return refreshItemsCmd; }
            set { SetProperty(ref refreshItemsCmd, value); }
        }


        private bool isLoadingMoreItems;
        public bool IsLoadingMoreItems
        {
            get { return isLoadingMoreItems; }
            set { SetProperty(ref isLoadingMoreItems, value); }
        }

        private bool showLoadMoreButton;
        public bool ShowLoadMoreButton
        {
            get { return showLoadMoreButton; }
            set { SetProperty(ref showLoadMoreButton, value); }
        }


        private void InitialConfigMessageListCollectionView()
        {
            var collectionView = this.messageListCollectionView;
            if (collectionView == null) return;

            var createTimeProp = nameof(UserMessage.CreateTimestamp);
            collectionView.SortDescriptions.Add(new SortDescription(createTimeProp, ListSortDirection.Descending));
            collectionView.LiveSortingProperties.Add(createTimeProp);
            collectionView.IsLiveSorting = true;
        }
    }
}
