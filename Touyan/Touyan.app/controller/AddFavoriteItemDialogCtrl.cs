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
using xueqiao.personal.user.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AddFavoriteItemDialogCtrl : IController
    {
        private readonly AddFavoriteItemVM contentVM;
        private readonly IMessageWindowService messageWindowService;
        private readonly ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl;
        
        private readonly DelegateCommand submitCmd;
        private readonly DelegateCommand cancelCmd;

        private IMessageWindow dialog;
        private bool isSubmiting;

        // 选中的收藏至的文件夹
        private long? selectedFavor2FolderId;

        [ImportingConstructor]
        public AddFavoriteItemDialogCtrl(
            AddFavoriteItemVM contentVM,
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
        /// 设置 dialog title。若不设置，则会根据情况设置默认 title
        /// </summary>
        public object DialogTitle { get; set; }

        /// <summary>
        /// 要添加的收藏项类型
        /// </summary>
        public ChartFolderListItemType FavoriteItemType { get; set; }

        /// <summary>
        /// 图表类型收藏的图表 id
        /// </summary>
        public long ChartId { get; set; }

        /// <summary>
        /// 初始的收藏项名称
        /// </summary>
        public string InitialFavoriteItemName { get; set; }

        /// <summary>
        /// 初始设置的父收藏夹 id。如果设置了该值。则不会显示收藏夹选择树
        /// </summary>
        public long? InitialParentFavoriteFolderId { get; set; }
        
        /// <summary>
        /// 已添加或修改的收藏项。
        /// </summary>
        public ChartFavoriteListItem AddOrUpdatedFavoriteItem { get; private set; }

        /// <summary>
        /// 收藏项是否是新添加的
        /// </summary>
        public bool? IsFavoriteItemNewAdded { get; private set; }

        public void Initialize()
        {
            this.selectedFavor2FolderId = this.InitialParentFavoriteFolderId;

            contentVM.HiddenFavorFolderSelectionView = IsHiddenFavorFolderSelectionView();
            contentVM.ViewWidth = IsHiddenFavorFolderSelectionView() ? 380 : 540;
            contentVM.FavoriteItemName = this.InitialFavoriteItemName;
            contentVM.SubmitCmd = submitCmd;
            contentVM.CancelCmd = cancelCmd;

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");
        }

        private bool IsHiddenFavorFolderSelectionView()
        {
            return this.InitialParentFavoriteFolderId != null;
        }

        public void Run()
        {
            LoadFavoriteFolderTree();

            var dialogTitle = this.DialogTitle;
            if (dialogTitle == null)
            {
                dialogTitle = this.FavoriteItemType == ChartFolderListItemType.Folder ? "添加收藏夹" : "添加收藏";
            }

            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, false,
                dialogTitle, contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged, "");
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddFavoriteItemVM.FavoriteItemName))
            {
                submitCmd?.RaiseCanExecuteChanged();
            }
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
            if (string.IsNullOrEmpty(contentVM.FavoriteItemName?.Trim()))
                return false;
            return true;
        }

        public void Submit()
        {
            if (isSubmiting) return;

            var favorItemName = contentVM.FavoriteItemName?.Trim();
            var favor2FolderId = this.selectedFavor2FolderId;
            if (favor2FolderId == null) return;

            var handleAddFavorRespExp = new Func<IInterfaceInteractResponse, bool>(_resp =>
            {
                if (_resp?.SourceException != null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(_resp, "添加收藏失败！\n");
                        messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, null, errMsg);
                    });
                    return true;
                }
                return false;
            });
            
            if (this.FavoriteItemType == ChartFolderListItemType.Folder)
            {
                var addFavorInfo = new FavoriteFolder { ParentFolderId = favor2FolderId.Value, FolderName = favorItemName };

                UpdateIsSubmiting(true);
                chartFavoriteServiceCtrl.RequestAddFavoriteFolder(addFavorInfo)?
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    UpdateIsSubmiting(false);
                    if (!handleAddFavorRespExp(resp?.Resp))
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() => 
                        {
                            this.AddOrUpdatedFavoriteItem = resp?.FavoriteItem;
                            this.IsFavoriteItemNewAdded = resp?.IsNewAdded;
                            InternalCloseDialog();
                        });
                    }
                });
            }
            else if (this.FavoriteItemType == ChartFolderListItemType.Chart)
            {
                var addFavorInfo = new FavoriteChart { XiaohaChartId = this.ChartId, ParentFolderId = favor2FolderId.Value, Name = favorItemName };

                UpdateIsSubmiting(true);
                chartFavoriteServiceCtrl.RequestAddFavoriteChart(addFavorInfo)?
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    UpdateIsSubmiting(false);
                    if (!handleAddFavorRespExp(resp?.Resp))
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            this.AddOrUpdatedFavoriteItem = resp?.FavoriteItem;
                            this.IsFavoriteItemNewAdded = resp?.IsNewAdded;
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
            if (rootFavorFolder != null)
            {
                var rootFolderNode = DMConstructHelper.GenerateFavoriteTreeNodeByRootFolderId(resultData, rootFavorFolder.FolderId, ChartFolderListItemType.Chart);
                folderNodeList.Add(rootFolderNode);
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
