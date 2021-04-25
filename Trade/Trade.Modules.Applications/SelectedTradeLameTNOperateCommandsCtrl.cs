using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Interfaces.Applications;

namespace XueQiaoWaf.Trade.Modules.Applications
{
    /// <summary>
    /// 选中的成交瘸腿 Task Note 项 <see cref="XueQiaoFoundation.BusinessResources.DataModels.XQTradeLameTaskNote"/>操作 command 控制。
    /// 在引用方的生命周期结束前需要保持该实例，才能有效控制选中项的操作 command
    /// </summary>
    [Export, Export(typeof(ISelectedTradeLameTNOperateCommandsCtrl)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SelectedTradeLameTNOperateCommandsCtrl : ISelectedTradeLameTNOperateCommandsCtrl
    {
        private readonly IXQTradeLameTaskNoteCtrl _XQTradeLameTaskNoteCtrl;

        private readonly DelegateCommand tradeLameTNSelectionChangedCmd;
        private readonly DelegateCommand deleteSelectedTNsCmd;

        private IEnumerable<XQTradeLameTaskNote> selectedItems;

        [ImportingConstructor]
        public SelectedTradeLameTNOperateCommandsCtrl(IXQTradeLameTaskNoteCtrl _XQTradeLameTaskNoteCtrl)
        {
            this._XQTradeLameTaskNoteCtrl = _XQTradeLameTaskNoteCtrl;

            tradeLameTNSelectionChangedCmd = new DelegateCommand(ItemsSelectionChanged);
            deleteSelectedTNsCmd = new DelegateCommand(DeleteSelectedItems, CanDeleteSelectedItems);

            this.SelectedTradeLameTNOptCommands = new SelectedTradeLameTNOperateCommands
            {
                TradeLameTNSelectionChangedCmd = tradeLameTNSelectionChangedCmd,
                DeleteSelectedTNsCmd = deleteSelectedTNsCmd,
            };
        }
        
        public SelectedTradeLameTNOperateCommands SelectedTradeLameTNOptCommands { get; private set; }

        private void ItemsSelectionChanged(object obj)
        {
            var oldSelItems = this.selectedItems;
            if (oldSelItems?.Any() == true)
            {
                foreach (var o in oldSelItems)
                {
                    PropertyChangedEventManager.RemoveHandler(o, TradeLameTNPropChanged, "");
                }
            }

            var newSelItems = (obj as IList)?.OfType<XQTradeLameTaskNote>().ToArray();
            if (newSelItems == null) return;
            
            this.selectedItems = newSelItems;
            if (newSelItems?.Any() == true)
            {
                foreach (var o in newSelItems)
                {
                    PropertyChangedEventManager.RemoveHandler(o, TradeLameTNPropChanged, "");
                    PropertyChangedEventManager.AddHandler(o, TradeLameTNPropChanged, "");
                }
            }

            RaiseCanExecuteChangedOfOptCmds();
        }

        private void TradeLameTNPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItemDataModel.OrderState)
                || e.PropertyName == nameof(OrderItemDataModel_Entrusted.EffectDate))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    RaiseCanExecuteChangedOfOptCmds();
                });
            }
        }

        private void RaiseCanExecuteChangedOfOptCmds()
        {
            deleteSelectedTNsCmd?.RaiseCanExecuteChanged();
        }

        private bool CanDeleteSelectedItems()
        {
            return selectedItems?.Any() == true;
        }

        private void DeleteSelectedItems()
        {
            var selItems = selectedItems?.ToArray();
            if (selItems?.Any() != true) return;

            foreach (var i in selItems)
            {
                _XQTradeLameTaskNoteCtrl.RequestDeleteTaskNote(new XQTradeItemKey(i.SubAccountId, i.XQTradeId));
            }
        }
    }
}
