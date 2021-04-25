using business_foundation_lib.helper;
using business_foundation_lib.xq_thriftlib_config;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using Thrift.Collections;
using Touyan.app.viewmodel;
using Touyan.Interface.application;
using Touyan.Interface.datamodel;
using xueqiao.personal.user.thriftapi;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;

namespace Touyan.app.controller
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class RenameFavoriteItemDialogCtrl
    {
        private readonly EditNameFavoriteItemVM contentVM;
        private readonly IMessageWindowService messageWindowService;
        private readonly ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl;
        private readonly ITouyanAuthUserLoginService authUserLoginService;

        private readonly DelegateCommand submitCmd;
        private readonly DelegateCommand cancelCmd;

        private IMessageWindow dialog;
        private bool isSubmiting;

        [ImportingConstructor]
        public RenameFavoriteItemDialogCtrl(
            EditNameFavoriteItemVM contentVM,
            IMessageWindowService messageWindowService,
            ITouyanChartFavoriteServiceCtrl chartFavoriteServiceCtrl,
            ITouyanAuthUserLoginService authUserLoginService)
        {
            this.contentVM = contentVM;
            this.messageWindowService = messageWindowService;
            this.chartFavoriteServiceCtrl = chartFavoriteServiceCtrl;
            this.authUserLoginService = authUserLoginService;

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
        /// 收藏项新的名称
        /// </summary>
        public string FavoriteItemNewName { get; private set; }

        public void Initialize()
        {
            if (FavoriteItemType != ChartFolderListItemType.Folder)
            {
                throw new NotSupportedException($"`{FavoriteItemType}` type is not supported.");
            }

            contentVM.ShowOriginNameRow = true;
            contentVM.SubmitCmd = submitCmd;
            contentVM.CancelCmd = cancelCmd;

            PropertyChangedEventManager.AddHandler(contentVM, ContentVMPropChanged, "");
        }

        public void Run()
        {
            RefreshFavoriteInfo();

            dialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, false,
                "收藏项重命名", contentVM.View);
            dialog.ShowDialog();
        }

        public void Shutdown()
        {
            InternalCloseDialog();
            PropertyChangedEventManager.RemoveHandler(contentVM, ContentVMPropChanged, "");
        }

        private void ContentVMPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditNameFavoriteItemVM.FavoriteItemNewName))
            {
                submitCmd?.RaiseCanExecuteChanged();
            }
        }


        private void RefreshFavoriteInfo()
        {
            var landinfoInfo = authUserLoginService.TouyanAuthUserLoginLandingInfo;
            if (landinfoInfo == null) return;

            Task.Run(() =>
            {
                var favoriteId = this.FavoriteId;
                string originName = null;

                if (this.FavoriteItemType == ChartFolderListItemType.Folder)
                {
                    var option = new ReqFavoriteFolderOption { FolderIds = new THashSet<long> { favoriteId } };
                    var favorFolder = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                        .reqFavoriteFolder(landinfoInfo, option)?
                        .CorrectResult?.FirstOrDefault(i => i.FolderId == favoriteId);
                    originName = favorFolder?.FolderName;
                }
                else if (this.FavoriteItemType == ChartFolderListItemType.Chart)
                {
                    var option = new ReqFavoriteChartOption { FavoriteChartIds = new THashSet<long> { favoriteId } };
                    var favorChart = XqThriftLibConfigurationManager.SharedInstance.XiaohaChartTerminalAoHttpStub
                        .reqFavoriteChart(landinfoInfo, option)?
                        .CorrectResult?.FirstOrDefault(i => i.FavoriteChartId == favoriteId);
                    originName = favorChart?.Name;
                }

                contentVM.FavoriteItemOriginName = originName;
            });
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
            if (string.IsNullOrEmpty(contentVM.FavoriteItemNewName?.Trim()))
                return false;
            return true;
        }

        public void Submit()
        {
            if (isSubmiting) return;

            var favoriteId = this.FavoriteId;
            var favorItemName = contentVM.FavoriteItemNewName?.Trim();
            if (string.IsNullOrEmpty(favorItemName)) return;

            var handleAddFavorRespExp = new Func<IInterfaceInteractResponse, bool>(_resp =>
            {
                if (_resp?.SourceException != null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var errMsg = FoundationHelper.FormatResponseDisplayErrorMsg(_resp, "重命名失败！\n");
                        messageWindowService.ShowMessageDialog(UIHelper.GetWindowOfUIElement(contentVM.View), null, null, null, errMsg);
                    });
                    return true;
                }
                return false;
            });

            if (this.FavoriteItemType == ChartFolderListItemType.Folder)
            {
                UpdateIsSubmiting(true);
                chartFavoriteServiceCtrl.RequestRenameFavoriteFolder(favoriteId, favorItemName)?
                .ContinueWith(t =>
                {
                    var resp = t.Result;
                    UpdateIsSubmiting(false);
                    if (!handleAddFavorRespExp(resp))
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            this.FavoriteItemNewName = favorItemName;
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
    }
}
