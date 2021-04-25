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
using Touyan.app.datamodel;
using Touyan.app.helper;
using Touyan.app.viewmodel;
using Touyan.Interface.application;
using Touyan.Interface.datamodel;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class MoveFavoriteItemDialogCtrl : IController
    {
        private readonly MoveFavoriteItemVM contentVM;
        private readonly IMessageWindowService messageWindowService;
        private readonly ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl;
        
        private readonly DelegateCommand submitCmd;
        private readonly DelegateCommand cancelCmd;

        private IMessageWindow dialog;
        private bool isSubmiting;

        // 选中的收藏至的文件夹
        private long? selectedFavor2FolderId;

        [ImportingConstructor]
        public MoveFavoriteItemDialogCtrl(
            MoveFavoriteItemVM contentVM,
            IMessageWindowService messageWindowService,
            ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl)
        {
            this.contentVM = contentVM;
            this.messageWindowService = messageWindowService;
            this.chartFavoriteServiceCtrl = chartFavoriteServiceCtrl;

            submitCmd = new DelegateCommand(Submit, CanSubmit);
            cancelCmd = new DelegateCommand(LeaveDialog);
        }
        
        public object DialogOwner { get; set; }

        /// <summary>
        /// 要移动的收藏项类型
        /// </summary>
        public ChartFolderListItemType FavoriteItemType { get; set; }

        /// <summary>
        /// 要移动的收藏项 id
        /// </summary>
        public long FavoriteId { get; set; }

        /// <summary>
        /// 移动到的父收藏夹 id
        /// </summary>
        public long? Moved2ParentFavorFolderId { get; private set; }

        public void Initialize()
        {
            contentVM.SubmitCmd = submitCmd;
            contentVM.CancelCmd = cancelCmd;
        }

        public void Run()
        {
            LoadFavoriteFolderTree();

            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, false,
                "收藏项移动", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
        }
        
        private void InternalCloseDialog()
        {
            if (dialog != null)
            {
                dialog.Close();
                dialog = null;
            }
        }

        public bool CanSubmit()
        {
            if (isSubmiting) return false;
            if (selectedFavor2FolderId == null) return false;
            return true;
        }

        public void Submit()
        {
            if (isSubmiting) return;
            
            var favor2FolderId = this.selectedFavor2FolderId;
            if (favor2FolderId == null) return;

            var handleAddFavorRespExp = new Func<IInterfaceInteractResponse, bool>(_resp =>
            {
                if (_resp?.SourceException != null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(_resp, "收藏项移动失败！\n");
                        messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, null, errMsg);
                    });
                    return true;
                }
                return false;
            });

            if (this.FavoriteItemType == ChartFolderListItemType.Folder)
            {
                if (favor2FolderId == this.FavoriteId)
                {
                    messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, null, 
                        "不能将自己移动到自己收藏夹中");
                    return;
                }

                UpdateIsSubmiting(true);
                chartFavoriteServiceCtrl.RequestMoveFavoriteFolder(this.FavoriteId, favor2FolderId.Value)?
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    UpdateIsSubmiting(false);
                    if (!handleAddFavorRespExp(resp))
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            this.Moved2ParentFavorFolderId = favor2FolderId;
                            InternalCloseDialog();
                        });
                    }
                });
            }
            else if (this.FavoriteItemType == ChartFolderListItemType.Chart)
            {
                UpdateIsSubmiting(true);
                chartFavoriteServiceCtrl.RequestMoveFavoriteChart(this.FavoriteId, favor2FolderId.Value)?
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    UpdateIsSubmiting(false);
                    if (!handleAddFavorRespExp(resp))
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            this.Moved2ParentFavorFolderId = favor2FolderId;
                            InternalCloseDialog();
                        });
                    }
                });
            }
        }

        private void LeaveDialog()
        {
            InternalCloseDialog();
        }
        
        private void UpdateIsSubmiting(bool value)
        {
            this.isSubmiting = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                submitCmd?.RaiseCanExecuteChanged();
                cancelCmd?.RaiseCanExecuteChanged();
            });
        }

        private void UpdateSelectedFavor2FolderId(long? value)
        {
            this.selectedFavor2FolderId = value;
            DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                submitCmd?.RaiseCanExecuteChanged();
            });
        }

        private async void LoadFavoriteFolderTree()
        {
            var resultWrapper = await chartFavoriteServiceCtrl.RefreshFavoriteListIfNeed();
            var resultData = resultWrapper?.ResultData?.ToArray() ?? new ChartFavoriteListItem[] { };
            var rootFavorFolder = DMConstructHelper.GetMeaningfulRootChartFavoriteItem(resultData);

            var folderNodeList = new List<ChartFavoriteListTreeNode_Folder>();
            if (rootFavorFolder != null) {
                var rootFolderNode = DMConstructHelper.GenerateFavoriteTreeNodeByRootFolderId(resultData, rootFavorFolder.FolderId, ChartFolderListItemType.Chart);
                folderNodeList.Add(rootFolderNode);
                if (this.FavoriteItemType == ChartFolderListItemType.Folder)
                {
                    DMConstructHelper.RemoveFavoriteFolderItemInFolderNodeList(folderNodeList, this.FavoriteId);
                }
            }

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                var favorFolderTree = new ChartFavoriteNodeTree();

                if (folderNodeList != null)
                {
                    foreach (var folderNode in folderNodeList)
                    {
                        ExpandAllNodes(folderNode);
                        AddPropChangedHandler_IsSelected(folderNode);
                        favorFolderTree.NodeList.Add(folderNode);
                    }
                }

                contentVM.FavorFolderTree = favorFolderTree;
            });
        }
        
        private void ExpandAllNodes(ChartFavoriteListTreeNode_Folder rootNode)
        {
            if (rootNode == null) return;
            rootNode.IsExpanded = true;
            foreach (var child in rootNode.Children)
            {
                if (child is ChartFavoriteListTreeNode_Folder folderChild)
                {
                    ExpandAllNodes(folderChild);
                }
                else
                {
                    child.IsExpanded = true;
                }
            }
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
            var node = sender as ChartFavoriteListTreeNodeBase;
            if (node != null)
            {
                var favorItemId = node.FavoriteId;
                if (node.IsSelected)
                {
                    UpdateSelectedFavor2FolderId(favorItemId);
                }
            }
        }
    }
}
