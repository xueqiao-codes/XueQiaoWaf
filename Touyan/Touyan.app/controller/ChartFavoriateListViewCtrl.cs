using business_foundation_lib.helper;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using Touyan.app.datamodel;
using Touyan.app.helper;
using Touyan.app.viewmodel;
using Touyan.Interface.application;
using Touyan.Interface.datamodel;
using xueqiao.graph.xiaoha.chart.terminal.ao.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;
using XueQiaoFoundation.UI.Components.ToastNotification;
using XueQiaoFoundation.UI.Components.ToastNotification.Impl;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ChartFavoriateListViewCtrl : IController
    {
        private readonly ChartFavoriateListVM contentVM;
        private readonly ITouyanAuthUserLoginService authUserLoginService;
        private readonly ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl;
        private readonly ExportFactory<AddFavoriteItemDialogCtrl> addFavoriteItemDialogCtrlFactory;
        private readonly ExportFactory<RenameFavoriteItemDialogCtrl> renameFavoriteItemDialogCtrlFactory;
        private readonly ExportFactory<MoveFavoriteItemDialogCtrl> moveFavoriteItemDialogCtrlFactory;
        private readonly ExportFactory<SimpleMessageToastNDPVM> simpleMessageToastNDPVMFactory;

        private readonly DelegateCommand loginEntryCmd;
        private readonly DelegateCommand registerEntryCmd;
        private readonly DelegateCommand link2TouyanUserEntryCmd;
        
        private readonly DelegateCommand refreshFavoriteListCmd;
        private readonly DelegateCommand newTopLevelFolderCmd;
        private readonly DelegateCommand newChildFolderCmd;
        private readonly DelegateCommand moveFavoriteItemCmd;
        private readonly DelegateCommand renameFavoriteItemCmd;
        private readonly DelegateCommand removeFavoriteItemCmd;
        
        /// <summary>
        /// 收藏列表的有意义的 root node
        /// </summary>
        private ChartFavoriteListTreeNode_Folder meaningfulRootFolderNode;

        private NotifierWrapper_ControlPositionProvider viewToastNotifierWrapper;

        [ImportingConstructor]
        public ChartFavoriateListViewCtrl(
            ChartFavoriateListVM contentVM,
            ITouyanAuthUserLoginService authUserLoginService,
            ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl,
            ExportFactory<AddFavoriteItemDialogCtrl> addFavoriteItemDialogCtrlFactory,
            ExportFactory<RenameFavoriteItemDialogCtrl> renameFavoriteItemDialogCtrlFactory,
            ExportFactory<MoveFavoriteItemDialogCtrl> moveFavoriteItemDialogCtrlFactory,
            ExportFactory<SimpleMessageToastNDPVM> simpleMessageToastNDPVMFactory)
        {
            this.contentVM = contentVM;
            this.authUserLoginService = authUserLoginService;
            this.chartFavoriteServiceCtrl = chartFavoriteServiceCtrl;
            this.addFavoriteItemDialogCtrlFactory = addFavoriteItemDialogCtrlFactory;
            this.renameFavoriteItemDialogCtrlFactory = renameFavoriteItemDialogCtrlFactory;
            this.moveFavoriteItemDialogCtrlFactory = moveFavoriteItemDialogCtrlFactory;
            this.simpleMessageToastNDPVMFactory = simpleMessageToastNDPVMFactory;

            loginEntryCmd = new DelegateCommand(() => {
                authUserLoginService.ShowTouyanAuthUserLoginDialog(UIHelper.GetWindowOfUIElement(contentVM.View));
            });
            registerEntryCmd = new DelegateCommand(() => {
                authUserLoginService.ShowTouyanAuthUserRegisterDialog(UIHelper.GetWindowOfUIElement(contentVM.View));
            });
            link2TouyanUserEntryCmd = new DelegateCommand(() => {
                authUserLoginService.ShowTouyanAuthUserLink2TouyanUserDialog(UIHelper.GetWindowOfUIElement(contentVM.View));
            });

            refreshFavoriteListCmd = new DelegateCommand(() => RefreshFavoriteList(true));
            newTopLevelFolderCmd = new DelegateCommand(NewTopLevelFolder, CanNewTopLevelFolder);
            newChildFolderCmd = new DelegateCommand(NewChildFolder, CanNewChildFolder);
            moveFavoriteItemCmd = new DelegateCommand(MoveFavoriteItem);
            renameFavoriteItemCmd = new DelegateCommand(RenameFavoriteItem, CanRenameFavoriteItem);
            removeFavoriteItemCmd = new DelegateCommand(RemoveFavoriteItem);
        }
        
        public Action<long> SelectedChartHandler { get; set; }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
            authUserLoginService.TouyanAuthUserHasLogined += RecvTouyanAuthUserHasLogined;
            authUserLoginService.TouyanAuthUserHasLogouted += RecvTouyanAuthUserHasLogouted;
            authUserLoginService.TouyanAuthUserLink2TouyanUserStateChanged += RecvTouyanAuthUserLink2TouyanUserStateChanged;

            chartFavoriteServiceCtrl.TouyanChartFavoriteItemAdded += RecvTouyanChartFavoriteItemAdded;
            chartFavoriteServiceCtrl.TouyanChartFavoriteItemRemoved += RecvTouyanChartFavoriteItemRemoved;
            chartFavoriteServiceCtrl.TouyanChartFavoriteItemMoved += RecvTouyanChartFavoriteItemMoved;

            contentVM.HasUserRegisterFeature = authUserLoginService.HasFeature_UserDataManageRegister;
            contentVM.LoginEntryCmd = loginEntryCmd;
            contentVM.RegisterEntryCmd = registerEntryCmd;
            contentVM.Link2TouyanUserEntryCmd = link2TouyanUserEntryCmd;

            contentVM.RefreshFavoriteListCmd = refreshFavoriteListCmd;
            contentVM.NewTopLevelFolderCmd = newTopLevelFolderCmd;
            contentVM.NewChildFolderCmd = newChildFolderCmd;
            contentVM.MoveFavoriteItemCmd = moveFavoriteItemCmd;
            contentVM.RenameFavoriteItemCmd = renameFavoriteItemCmd;
            contentVM.RemoveFavoriteItemCmd = removeFavoriteItemCmd;
        }
        
        public void Run()
        {
            InvalidatePageContent();
        }

        public void Shutdown()
        {
            DisposeViewToastNotifierWrapper();

            authUserLoginService.TouyanAuthUserHasLogined -= RecvTouyanAuthUserHasLogined;
            authUserLoginService.TouyanAuthUserHasLogouted -= RecvTouyanAuthUserHasLogouted;
            authUserLoginService.TouyanAuthUserLink2TouyanUserStateChanged -= RecvTouyanAuthUserLink2TouyanUserStateChanged;

            chartFavoriteServiceCtrl.TouyanChartFavoriteItemAdded -= RecvTouyanChartFavoriteItemAdded;
            chartFavoriteServiceCtrl.TouyanChartFavoriteItemRemoved -= RecvTouyanChartFavoriteItemRemoved;
            chartFavoriteServiceCtrl.TouyanChartFavoriteItemMoved -= RecvTouyanChartFavoriteItemMoved;
        }
        
        public void InvalidatePageContent()
        {
            InvalidateShowLink2TouyanUserGuideView();
            InvalidateAuthorUserHasLogined();
            RefreshFavoriteList(false);
        }
        
        private void RecvTouyanAuthUserHasLogined()
        {
            InvalidatePageContent();
        }

        private void RecvTouyanAuthUserHasLogouted(XiaohaChartLandingInfo lastLoginLandingInfo)
        {
            InvalidatePageContent();
        }

        private void RecvTouyanAuthUserLink2TouyanUserStateChanged(XiaohaChartLandingInfo authUserLoginLandingInfo, bool linkedState)
        {
            InvalidateShowLink2TouyanUserGuideView();
            RefreshFavoriteList(true);
        }

        private void RecvTouyanChartFavoriteItemAdded(ChartFavoriteListItem addItem)
        {
            if (addItem == null) return;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                var parentFolderNode = DMConstructHelper.GetFavoriteFolderNodeFromTree(addItem.ParentFavoriteFolderId, meaningfulRootFolderNode) as ChartFavoriteListTreeNode_Folder;
                if (parentFolderNode == null) return;

                var existAddNode = DMConstructHelper.GetFavoriteNodeFromTree(addItem.ItemType, addItem.FavoriteId, parentFolderNode);
                if (existAddNode != null) return;

                ChartFavoriteListTreeNodeBase newNode = null;
                if (addItem.ItemType == ChartFolderListItemType.Folder
                        && addItem is ChartFavoriteListItem_Folder folderData)
                {
                    newNode = new ChartFavoriteListTreeNode_Folder(folderData.FolderId)
                    {
                        FavoriteData = folderData
                    };
                }
                else if (addItem.ItemType == ChartFolderListItemType.Chart
                        && addItem is ChartFavoriteListItem_Chart chartData)
                {
                    newNode = new ChartFavoriteListTreeNode_Chart(chartData.FavoriteId, chartData.ChartId)
                    {
                        FavoriteData = chartData
                    };
                }

                if (newNode != null)
                {
                    AddPropChangedHandler_IsSelected(newNode);
                    parentFolderNode.Children.Insert(0, newNode);
                }
            });
        }

        private void RecvTouyanChartFavoriteItemRemoved(ChartFavoriteListItem rmItem)
        {
            if (rmItem == null) return;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                // 遍历当前树，删除节点
                DMConstructHelper.RemoveFavoriteNodeRecursive(this.meaningfulRootFolderNode?.Children, rmItem.ItemType, rmItem.FavoriteId);
            });
        }

        private void RecvTouyanChartFavoriteItemMoved(ChartFavoriteListItem movedItem, long oldParentFolderId)
        {
            if (movedItem == null) return;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                var newParentFavorFolderId = movedItem.ParentFavoriteFolderId;
                if (newParentFavorFolderId != oldParentFolderId)
                {
                    var movedNode = DMConstructHelper.GetFavoriteNodeFromTree(movedItem.ItemType, movedItem.FavoriteId, meaningfulRootFolderNode);
                    if (movedNode != null)
                    {
                        // 从原来的父节点删除
                        var originParentFolderNode = DMConstructHelper.GetFavoriteFolderNodeFromTree(oldParentFolderId, meaningfulRootFolderNode) as ChartFavoriteListTreeNode_Folder;
                        if (originParentFolderNode != null)
                        {
                            originParentFolderNode.Children.Remove(movedNode);
                        }

                        // 添加至新的父节点
                        var newParentFolderNode = DMConstructHelper.GetFavoriteFolderNodeFromTree(newParentFavorFolderId, meaningfulRootFolderNode) as ChartFavoriteListTreeNode_Folder;
                        if (newParentFolderNode != null)
                        {
                            AddPropChangedHandler_IsSelected(movedNode);
                            newParentFolderNode.Children.Insert(0, movedNode);
                        }
                    }
                }
            });
        }

        private XqToastNotification CreateSimpleMessageToastNotification(string message)
        {
            var noteNDPVM = simpleMessageToastNDPVMFactory.CreateExport().Value;
            noteNDPVM.MessageContent = message;
            return new XqToastNotification(noteNDPVM, false);
        }

        private NotifierWrapper_ControlPositionProvider AcquireViewToastNotifierWrapper()
        {
            if (viewToastNotifierWrapper == null)
            {
                viewToastNotifierWrapper = new NotifierWrapper_ControlPositionProvider(contentVM.View as FrameworkElement, Corner.BottomCenter, 0, 20,
                    _confWrapper =>
                    {
                        _confWrapper.LifetimeSupervisor = new Tuple<INotificationsLifetimeSupervisor>(new TimeAndFIFONotificationLifetimeSupervisor(TimeSpan.FromSeconds(3), 1));
                        _confWrapper.DisplayOptions = new Tuple<DisplayOptions>(new DisplayOptions { TopMost = false, Width = 280 });
                    });
            }
            return viewToastNotifierWrapper;
        }

        private void DisposeViewToastNotifierWrapper()
        {
            if (viewToastNotifierWrapper != null)
            {
                viewToastNotifierWrapper.Dispose();
                viewToastNotifierWrapper = null;
            }
        }

        private bool CanNewTopLevelFolder()
        {
            return this.meaningfulRootFolderNode != null;
        }

        private void NewTopLevelFolder()
        {
            var rootFolderNode = this.meaningfulRootFolderNode;
            if (rootFolderNode == null) return;

            // 添加顶层收藏夹
            var dialogCtrl = addFavoriteItemDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            dialogCtrl.DialogTitle = "添加收藏夹";
            dialogCtrl.FavoriteItemType = ChartFolderListItemType.Folder;
            dialogCtrl.InitialParentFavoriteFolderId = rootFolderNode.FolderId;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();

            // Warning:在 TouyanChartFavoriteItemAdded 事件中进行添加处理
        }

        private bool CanNewChildFolder(object obj)
        {
            var favorItem = obj as ChartFavoriteListTreeNode_Folder;
            if (favorItem == null) return false;
            return true;
        }

        private void NewChildFolder(object obj)
        {
            var favorItem = obj as ChartFavoriteListTreeNode_Folder;
            if (favorItem == null) return;

            // 添加子收藏夹
            var dialogCtrl = addFavoriteItemDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            dialogCtrl.DialogTitle = "添加子收藏夹";
            dialogCtrl.FavoriteItemType = ChartFolderListItemType.Folder;
            dialogCtrl.InitialParentFavoriteFolderId = favorItem.FolderId;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();

            // Warning:在 TouyanChartFavoriteItemAdded 事件中进行添加处理
            if (dialogCtrl.AddOrUpdatedFavoriteItem != null)
            {
                favorItem.IsExpanded = true;
            }
        }

        private void MoveFavoriteItem(object obj)
        {
            var favorItem = obj as ChartFavoriteListTreeNodeBase;
            if (favorItem == null) return;

            // 移动
            ChartFolderListItemType? favorItemType = null;
            long? originParentFolderId = null;

            if (favorItem is ChartFavoriteListTreeNode_Folder favorFolderNode)
            {
                favorItemType = ChartFolderListItemType.Folder;
                originParentFolderId = favorFolderNode.FavoriteData?.ParentFavoriteFolderId;
            }
            else if (favorItem is ChartFavoriteListTreeNode_Chart favorChartNode)
            {
                favorItemType = ChartFolderListItemType.Chart;
                originParentFolderId = favorChartNode.FavoriteData?.ParentFavoriteFolderId;
            }

            if (favorItemType != null && originParentFolderId != null)
            {
                var dialogCtrl = moveFavoriteItemDialogCtrlFactory.CreateExport().Value;
                dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);

                dialogCtrl.FavoriteItemType = favorItemType.Value;
                dialogCtrl.FavoriteId = favorItem.FavoriteId;

                dialogCtrl.Initialize();
                dialogCtrl.Run();
                dialogCtrl.Shutdown();

                // Warning:在 TouyanChartFavoriteItemMoved 事件中进行移动处理
                var moved2ParentFavorFolderId = dialogCtrl.Moved2ParentFavorFolderId;
                if (moved2ParentFavorFolderId != null && moved2ParentFavorFolderId != originParentFolderId)
                {
                    // 展开新的父节点
                    var newParentFolderNode = DMConstructHelper.GetFavoriteFolderNodeFromTree(moved2ParentFavorFolderId.Value, meaningfulRootFolderNode) as ChartFavoriteListTreeNode_Folder;
                    if (newParentFolderNode != null)
                    {
                        newParentFolderNode.IsExpanded = true;
                    }
                }
            }
        }

        private bool CanRenameFavoriteItem(object obj)
        {
            return (obj is ChartFavoriteListTreeNode_Folder);
        }

        private void RenameFavoriteItem(object obj)
        {
            var favorItem = obj as ChartFavoriteListTreeNode_Folder;
            if (favorItem == null) return;

            // 重命名
            var dialogCtrl = renameFavoriteItemDialogCtrlFactory.CreateExport().Value;
            dialogCtrl.DialogOwner = UIHelper.GetWindowOfUIElement(contentVM.View);
            dialogCtrl.FavoriteItemType = ChartFolderListItemType.Folder;
            dialogCtrl.FavoriteId = favorItem.FolderId;

            dialogCtrl.Initialize();
            dialogCtrl.Run();
            dialogCtrl.Shutdown();
        }

        private void RemoveFavoriteItem(object obj)
        {
            var handlerRemoveResp = new Action<ChartFavoriteListTreeNodeBase, IInterfaceInteractResponse>((_rmItem, _rmResp) =>
            {
                if (_rmItem == null) return;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (_rmResp?.SourceException != null)
                    {
                        var ntf = CreateSimpleMessageToastNotification(FoundationHelper.FormatResponseDisplayErrorMsg(_rmResp, "删除失败！\n"));
                        AcquireViewToastNotifierWrapper().Notify<XqToastNotification>(() => ntf);
                        return;
                    }

                    // Warning:在 TouyanChartFavoriteItemRemoved 事件中进行删除处理
                });
            });

            if (obj is ChartFavoriteListTreeNode_Folder folderTypeItem)
            {
                chartFavoriteServiceCtrl.RequestRemoveFavoriteFolder(folderTypeItem.FolderId)
                .ContinueWith(t =>
                {
                    handlerRemoveResp(folderTypeItem, t.Result);
                });
            }
            else if (obj is ChartFavoriteListTreeNode_Chart chartTypeItem)
            {
                chartFavoriteServiceCtrl.RequestRemoveFavoriteChart(chartTypeItem.FavoriteId)
                .ContinueWith(t =>
                {
                    handlerRemoveResp(chartTypeItem, t.Result);
                });
            }
        }
        
        private void InvalidateShowLink2TouyanUserGuideView()
        {
            contentVM.ShowLink2TouyanUserGuideView = authUserLoginService.HasFeature_UserDataManageLink2TouyanUser && !authUserLoginService.HasLinked2TouyanUserOfTouyanAuthUser;
        }

        private void InvalidateAuthorUserHasLogined()
        {
            contentVM.AuthorUserHasLogined = authUserLoginService.TouyanAuthUserLoginLandingInfo != null;
        }

        private async void RefreshFavoriteList(bool forceRefresh)
        {
            ServerDataRefreshResultWrapper<IEnumerable<ChartFavoriteListItem>> resultWrapper = null;
            if (forceRefresh)
            {
                resultWrapper = await chartFavoriteServiceCtrl.RefreshFavoriteListForce();
            }
            else
            {
                resultWrapper = await chartFavoriteServiceCtrl.RefreshFavoriteListIfNeed();
            }
            var resultData = resultWrapper?.ResultData?.ToArray() ?? new ChartFavoriteListItem[] { };

            var rootFavorFolder = DMConstructHelper.GetMeaningfulRootChartFavoriteItem(resultData);
            ChartFavoriteListTreeNode_Folder favorFolderRootNode = null;
            if (rootFavorFolder != null)
            {
                favorFolderRootNode = DMConstructHelper.GenerateFavoriteTreeNodeByRootFolderId(resultData, rootFavorFolder.FolderId);
            }

            this.meaningfulRootFolderNode = favorFolderRootNode;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                contentVM.FavoriteListRootFolderNode = this.meaningfulRootFolderNode;
                var childrenNodes = contentVM.FavoriteListRootFolderNode?.Children.ToArray();
                if (childrenNodes != null)
                {
                    foreach (var _node in childrenNodes)
                    {
                        AddPropChangedHandler_IsSelected(_node);
                    }
                }

                AllCommandsRaiseCanExecuteChanged();
            });
        }
        
        private void AllCommandsRaiseCanExecuteChanged()
        {
            loginEntryCmd?.RaiseCanExecuteChanged();
            registerEntryCmd?.RaiseCanExecuteChanged();
            link2TouyanUserEntryCmd?.RaiseCanExecuteChanged();

            refreshFavoriteListCmd?.RaiseCanExecuteChanged();
            newTopLevelFolderCmd?.RaiseCanExecuteChanged();
            newChildFolderCmd?.RaiseCanExecuteChanged();
            renameFavoriteItemCmd?.RaiseCanExecuteChanged();
            removeFavoriteItemCmd?.RaiseCanExecuteChanged();
        }

        private void AddPropChangedHandler_IsSelected(ChartFavoriteListTreeNodeBase rootNode)
        {
            if (rootNode == null) return;

            PropertyChangedEventManager.RemoveHandler(rootNode, FavoriteListTreeNodeChanged_IsSelected, nameof(ChartFavoriteListTreeNodeBase.IsSelected));
            PropertyChangedEventManager.AddHandler(rootNode, FavoriteListTreeNodeChanged_IsSelected, nameof(ChartFavoriteListTreeNodeBase.IsSelected));

            if (rootNode is ChartFavoriteListTreeNode_Folder folderNode)
            {
                foreach (var child in folderNode.Children)
                {
                    AddPropChangedHandler_IsSelected(child);
                }
            }
        }

        private void FavoriteListTreeNodeChanged_IsSelected(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ChartFavoriteListTreeNode_Chart chartNode && chartNode.IsSelected)
            {
                SelectedChartHandler?.Invoke(chartNode.ChartId);
            }
        }
    }
}
