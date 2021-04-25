using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.trade.hosting.proxy;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

using XueQiaoWaf.Trade.Interfaces.Applications;
using XueQiaoWaf.Trade.Modules.Applications.Controllers.Events;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Helper;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 委托单列表显示列配置弹窗 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class DisplayColumnsOrderListConditionConfigDialogController : IController
    {
        private readonly IMessageWindowService messageWindowService;
        private readonly IEventAggregator eventAggregator;
        private readonly ITradeModuleService tradeModuleService;
        private readonly Lazy<ILoginUserManageService> loginUserManageService;
        private readonly DisplayColumnsOrderListConditionConfigItemsContainerModel configItemsContainerModel;
        private readonly ListDisplayColumnsConfigDialogContentViewModel<ListDisplayColumnConfigureItem> dialogContentViewModel;

        private readonly DelegateCommand resetToDefaultDisplayColumnsCmd;
        private readonly DelegateCommand saveCmd;
        private readonly DelegateCommand cancelCmd;

        private IMessageWindow configureDialog;

        [ImportingConstructor]
        public DisplayColumnsOrderListConditionConfigDialogController(
            IMessageWindowService messageWindowService,
            IEventAggregator eventAggregator,
            ITradeModuleService tradeModuleService,
            Lazy<ILoginUserManageService> loginUserManageService,
            DisplayColumnsOrderListConditionConfigItemsContainerModel configItemsContainerModel,
            ListDisplayColumnsConfigDialogContentViewModel<ListDisplayColumnConfigureItem> dialogContentViewModel)
        {
            this.messageWindowService = messageWindowService;
            this.eventAggregator = eventAggregator;
            this.tradeModuleService = tradeModuleService;
            this.loginUserManageService = loginUserManageService;
            this.configItemsContainerModel = configItemsContainerModel;
            this.dialogContentViewModel = dialogContentViewModel;

            resetToDefaultDisplayColumnsCmd = new DelegateCommand(ResetToDefaultDisplayColumns);
            saveCmd = new DelegateCommand(SaveConfiguration);
            cancelCmd = new DelegateCommand(LeaveDialog);

            loginUserManageService.Value.HasLogouted += RecvUserHasLogouted;
        }

        public object DialogOwner { get; set; }

        /// <summary>
        /// 原先显示的列
        /// </summary>
        public IEnumerable<ListColumnInfo> OriginDisplayingColumnInfos { get; set; }

        /// <summary>
        /// 显示列的配置结果
        /// </summary>
        public IEnumerable<ListColumnInfo> ConfiguredDisplayColomunsResult { get; private set; }

        public void Initialize()
        {
            if (DialogOwner == null) throw new ArgumentNullException("DialogOwner");

            PropertyChangedEventManager.AddHandler(configItemsContainerModel, ConfigItemsContainerModelPropertyChanged, "");

            dialogContentViewModel.ApplyAsGlobalText = "应用到全部条件单列表";
            dialogContentViewModel.ResetToDefaultDisplayColumnsCmd = resetToDefaultDisplayColumnsCmd;
            dialogContentViewModel.SaveCmd = saveCmd;
            dialogContentViewModel.CancelCmd = cancelCmd;

            dialogContentViewModel.DisplayColumnsConfigItemsContainerView = configItemsContainerModel.View;
            dialogContentViewModel.ConfigItemMoveCommands = new ListDisplayColumnsConfigItemMoveCommands<ListDisplayColumnConfigureItem>(configItemsContainerModel.ColumnConfigItems)
            {
                SelectedConfigItem = configItemsContainerModel.SelectedConfigItem
            };
            DisplayColumnsConfigItemsReset(OriginDisplayingColumnInfos);
        }

        public void Run()
        {
            configureDialog = messageWindowService.CreateContentCustomWindow(DialogOwner, null, null, true, false, 
                true, "条件单列表列配置", dialogContentViewModel.View);
            configureDialog.ShowDialog();
        }

        public void Shutdown()
        {
            if (configureDialog != null)
            {
                configureDialog.Close();
                configureDialog = null;
            }
            loginUserManageService.Value.HasLogouted -= RecvUserHasLogouted;
            PropertyChangedEventManager.RemoveHandler(configItemsContainerModel, ConfigItemsContainerModelPropertyChanged, "");
        }

        private void RecvUserHasLogouted(ProxyLoginResp lastLoginResp)
        {
            Shutdown();
        }

        private void ConfigItemsContainerModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DisplayColumnsOrderListConditionConfigItemsContainerModel.SelectedConfigItem))
            {
                // 更新操作命令合集对象的选中项
                var configItemMoveCommands = dialogContentViewModel.ConfigItemMoveCommands;
                if (configItemMoveCommands != null)
                {
                    configItemMoveCommands.SelectedConfigItem = configItemsContainerModel.SelectedConfigItem;
                }
            }
        }

        private void DisplayColumnsConfigItemsReset(IEnumerable<ListColumnInfo> toDisplayColumnInfos)
        {
            if (toDisplayColumnInfos == null) toDisplayColumnInfos = new ListColumnInfo[] { };

            var allColumnsNames = Enum.GetNames(typeof(OrderListColumn_Condition));
            var notDisplayColumnCodes = allColumnsNames
                .Select(n =>
                {
                    Enum.TryParse(n, out OrderListColumn_Condition column);
                    return column.GetHashCode();
                })
                .Except(toDisplayColumnInfos?.Select(i => i.ColumnCode).ToArray());

            var configureColumnItems = toDisplayColumnInfos.Select(column => new ListDisplayColumnConfigureItem(column)
            {
                IsToDisplay = true
            })
            .ToArray()
            .Union(notDisplayColumnCodes.Select(columnCode => new ListDisplayColumnConfigureItem(new ListColumnInfo { ColumnCode = columnCode })
            {
                IsToDisplay = false
            }));

            configItemsContainerModel.ColumnConfigItems.Clear();
            configItemsContainerModel.ColumnConfigItems.AddRange(configureColumnItems);
        }
    
        private void ResetToDefaultDisplayColumns()
        {
        var defaultDisplayColumnCodes = TradeWorkspaceDataDisplayHelper.DefaultOrderListConditionDisplayColumns.Select(i => i.GetHashCode()).ToArray();
        var currentDisplayColumnCodes = configItemsContainerModel.ColumnConfigItems.Where(i => i.IsToDisplay).Select(i => i.Column.ColumnCode).ToArray();
        if (currentDisplayColumnCodes.SequenceEqual(defaultDisplayColumnCodes)) return;

        var resetItems = new List<ListColumnInfo>();
        foreach (var columnCode in defaultDisplayColumnCodes)
        {
            var columnInfo = configItemsContainerModel.ColumnConfigItems.FirstOrDefault(i => i.Column.ColumnCode == columnCode)?.Column;
            if (columnInfo == null)
            {
                columnInfo = new ListColumnInfo { ColumnCode = columnCode };
            }
            resetItems.Add(columnInfo);
        }

        DisplayColumnsConfigItemsReset(resetItems);
    }

        private void SaveConfiguration()
        {
            var configuredColumns = configItemsContainerModel.ColumnConfigItems
                .Where(i => i.IsToDisplay)
                .Select(i => i.Column)
                .ToArray();

            if (dialogContentViewModel.IsApplyAsGlobal)
            {
                // 保存配置项
                var dataRoot = tradeModuleService.TradeWorkspaceDataRoot;
                if (dataRoot != null)
                {
                    dataRoot.GlobalAppliedOrderListConditionDisplayColumns
                        = configuredColumns.Select(i => (ListColumnInfo)i.Clone()).ToArray();
                }
                // 发布一个事件
                eventAggregator.GetEvent<GlobalApplyOrderListConditionColumnsEvent>().Publish(configuredColumns);
            }

            this.ConfiguredDisplayColomunsResult = configuredColumns;
            // 关闭窗口
            dialogContentViewModel.CloseDisplayInWindow();
        }

        private void LeaveDialog()
        {
            this.ConfiguredDisplayColomunsResult = null;
            dialogContentViewModel.CloseDisplayInWindow();
        }
    }
}
