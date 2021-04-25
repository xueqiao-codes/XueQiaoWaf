using ContainerShell.Applications.ViewModels;
using ContainerShell.Interfaces.Applications;
using ContainerShell.Interfaces.Services;
using lib.xqclient_base.logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Waf.Applications;
using System.Windows;
using xueqiao.mailbox.user.message.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace ContainerShell.Applications.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class UserMessagePanelWindowCtrl : IController
    {
        private const string _404HtmlContent = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.0 Transitional//EN\">" +
            "<html><head><title>404</title></head><body><H1>Not Found</H1></body></html>";

        private readonly UserMessagePanelVM contentVM;
        private readonly UserMessageContentVM messageContentVM;
        private readonly IMessageWindowService messageWindowService;
        private readonly IUserMessageServiceCtrl userMessageServiceCtrl;

        private readonly TaskFactory updateMessagContentTaskFactory = new TaskFactory(new OrderedTaskScheduler());

        private readonly DelegateCommand loadMoreItemsCmd;
        private readonly DelegateCommand refreshItemsCmd;

        private IMessageWindow window;
        private bool windowIsShow = false;

        private bool isLoadingMore;
        private bool isRefreshing;

        [ImportingConstructor]
        public UserMessagePanelWindowCtrl(
            UserMessagePanelVM contentVM,
            UserMessageContentVM messageContentVM,
            IMessageWindowService messageWindowService,
            IUserMessageServiceCtrl userMessageServiceCtrl)
        {
            this.contentVM = contentVM;
            this.messageContentVM = messageContentVM;
            this.messageWindowService = messageWindowService;
            this.userMessageServiceCtrl = userMessageServiceCtrl;

            loadMoreItemsCmd = new DelegateCommand(LoadMoreItems, CanLoadMoreItems);
            refreshItemsCmd = new DelegateCommand(RefreshItems, CanRefreshItems);
        }
        
        /// <summary>
        /// 窗口 owner 
        /// </summary>
        public object WindowOwner { get; set; }
        
        /// <summary>
        /// 已关闭的处理
        /// </summary>
        public Action ClosedHandler { get; set; }

        /// <summary>
        /// 窗口是否显示
        /// </summary>
        public bool WindowIsShow => this.windowIsShow;
        
        public void Initialize()
        {
            var winSize = new Size(SystemParameters.PrimaryScreenWidth * 0.8, SystemParameters.PrimaryScreenHeight * 0.8);
            this.window = messageWindowService.CreateCompleteCustomWindow(WindowOwner, null, winSize, false, true, contentVM.View, contentVM.WindowCaptionHeightHolder);
            this.window.Closed += Window_Closed;
            
            contentVM.CloseMenuButtonClickHandler = (s, e) =>
            {
                // 将关闭按钮点击处理成隐藏窗口
                Hide();
                e.Handled = true;
            };

            contentVM.SelectedMessageContentView = messageContentVM.View;
            contentVM.LoadMoreItemsCmd = loadMoreItemsCmd;
            contentVM.RefreshItemsCmd = refreshItemsCmd;
            contentVM.ShowLoadMoreButton = true;

            HandleSelectedMessageItemChanged();

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");
        }

        public void Run()
        {
            
        }

        public void Shutdown()
        {
            contentVM.CloseMenuButtonClickHandler = null;
            contentVM.SelectedMessageContentView = null;
            InternalClosePanelWindow();

            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged, "");
        }

        public void Show()
        {
            if (window != null)
            {
                window.Show(false);
                window.Activate();
                windowIsShow = true;
            }
        }

        public void Hide()
        {
            window?.Hide();
            windowIsShow = false;
            ActivateOwnerWindow();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            windowIsShow = false;
            Shutdown();
            ActivateOwnerWindow();
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserMessagePanelVM.SelectedMessageItem))
            {
                HandleSelectedMessageItemChanged();
            }
        }

        private void HandleSelectedMessageItemChanged()
        {
            var selItem = contentVM.SelectedMessageItem;

            messageContentVM.MessageItem = selItem;
            updateMessagContentTaskFactory.StartNew(() =>
            {
                if (selItem != null && selItem.State != MessageState.READ)
                {
                    userMessageServiceCtrl.MarkMessageAsRead(selItem.MessageId);
                }

                string msgContent = _404HtmlContent;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    messageContentVM.IsContentDownloading = true;
                });

                if (selItem != null)
                {
                    var selectedMsgContentFileName = selItem.ContentFileName;
                    if (!string.IsNullOrEmpty(selectedMsgContentFileName))
                    {
                        try
                        {
                            Uri uri = new Uri(selectedMsgContentFileName);
                            using (var webclient = new WebClient())
                            {
                                var bytes = webclient.DownloadData(uri);
                                if (bytes != null)
                                {
                                    msgContent = Encoding.UTF8.GetString(bytes);
                                    bytes = null;
                                }
                            }
                        }
                        catch (Exception _ex)
                        {
                            AppLog.Error($"Failed download message content with url ({selectedMsgContentFileName})", _ex);
                        }
                    }
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    messageContentVM.IsContentDownloading = false;
                    messageContentVM.UpdateContentWithHtml(msgContent);
                });
            });
        }

        private void ActivateOwnerWindow()
        {
            // This is a workaround. Without this line the main window might hide behind
            // another running application.
            var ownerWin = (WindowOwner as Window) ?? Application.Current.MainWindow;
            ownerWin?.Activate();
        }

        private void InternalClosePanelWindow()
        {
            try
            {
                if (window != null)
                {
                    window.Closed -= Window_Closed;
                    window?.Close();
                    windowIsShow = false;
                    ActivateOwnerWindow();
                }
            }
            catch (Exception e)
            { Console.WriteLine(e); }
        }

        private void UpdateIsLoadingMore(bool value)
        {
            this.isLoadingMore = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                contentVM.IsLoadingMoreItems = value;

                loadMoreItemsCmd?.RaiseCanExecuteChanged();
                refreshItemsCmd?.RaiseCanExecuteChanged();
            });
        }

        private void UpdateIsRefreshing(bool value)
        {
            this.isRefreshing = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                loadMoreItemsCmd?.RaiseCanExecuteChanged();
                refreshItemsCmd?.RaiseCanExecuteChanged();
            });
        }


        private bool CanLoadMoreItems()
        {
            return !isLoadingMore && !isRefreshing;
        }

        private void LoadMoreItems()
        {
            UpdateIsLoadingMore(true);
            userMessageServiceCtrl.LoadOldMessages(null).ContinueWith(t => 
            {
                try
                {
                    var resp = t.Result;
                    UpdateIsLoadingMore(false);
                    if (resp == null) return;

                    contentVM.ShowLoadMoreButton = (resp.CorrectResult?.TotalCount > resp.CorrectResult?.Page?.Count());
                }
                catch (AggregateException)
                {
                    UpdateIsLoadingMore(false);
                }
            });
        }

        private bool CanRefreshItems()
        {
            return !isRefreshing;
        }

        private void RefreshItems()
        {
            UpdateIsRefreshing(true);
            UpdateIsLoadingMore(false);
            userMessageServiceCtrl.RefreshMessageList(true).ContinueWith(t => 
            {
                try
                {
                    var resp = t.Result;
                    UpdateIsRefreshing(false);
                }
                catch (AggregateException)
                {
                    UpdateIsRefreshing(false);
                }
            });
        }
    }
}
