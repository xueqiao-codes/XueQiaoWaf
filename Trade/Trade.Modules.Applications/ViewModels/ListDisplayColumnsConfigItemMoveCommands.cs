using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    public class ListDisplayColumnsConfigItemMoveCommands<T> : Model
    {
        private readonly ObservableCollection<T> configureColumnItems;
        private readonly DelegateCommand moveSelectedItemToPreviousIndexCmd;
        private readonly DelegateCommand moveSelectedItemToNextIndexCmd;
        private readonly DelegateCommand moveSelectedItemToFirstIndexCmd;
        private readonly DelegateCommand moveSelectedItemToLastIndexCmd;

        public ListDisplayColumnsConfigItemMoveCommands(ObservableCollection<T> configureColumnItems)
        {
            this.configureColumnItems = configureColumnItems;
            moveSelectedItemToPreviousIndexCmd = new DelegateCommand(MoveSelectedItemToPreviousIndex, CanMoveSelectedItemToPreviousIndex);
            moveSelectedItemToNextIndexCmd = new DelegateCommand(MoveSelectedItemToNextIndex, CanMoveSelectedItemToNextIndex);
            moveSelectedItemToFirstIndexCmd = new DelegateCommand(MoveSelectedItemToFirstIndex, CanMoveSelectedItemToFirstIndex);
            moveSelectedItemToLastIndexCmd = new DelegateCommand(MoveSelectedItemToLastIndex, CanMoveSelectedItemToLastIndex);
        }

        private T selectedConfigItem;
        /// <summary>
        /// 选中的项
        /// </summary>
        public T SelectedConfigItem
        {
            get { return selectedConfigItem; }
            set
            {
                if (SetProperty(ref selectedConfigItem, value))
                {
                    MoveCommandsRaiseCanExcuteChanged();
                }
            }
        }

        /// <summary>
        /// 移动到上一个位置 command。command的参数为<see cref="ListDisplayColumnsConfigItemMoveCommands<T>"/>的泛型类型
        /// </summary>
        public ICommand MoveSelectedItemToPreviousIndexCmd => moveSelectedItemToPreviousIndexCmd;

        /// <summary>
        /// 移动到下一个位置 command。command的参数为<see cref="ListDisplayColumnsConfigItemMoveCommands<T>"/>的泛型类型
        /// </summary>
        public ICommand MoveSelectedItemToNextIndexCmd => moveSelectedItemToNextIndexCmd;

        /// <summary>
        /// 移动到第一个位置 command。command的参数为<see cref="ListDisplayColumnsConfigItemMoveCommands<T>"/>的泛型类型
        /// </summary>
        public ICommand MoveSelectedItemToFirstIndexCmd => moveSelectedItemToFirstIndexCmd;

        /// <summary>
        /// 移动到最后一个位置 command。command的参数为<see cref="ListDisplayColumnsConfigItemMoveCommands<T>"/>的泛型类型
        /// </summary>
        public ICommand MoveSelectedItemToLastIndexCmd => moveSelectedItemToLastIndexCmd;

        private void MoveSelectedItemToPreviousIndex()
        {
            if (SelectedConfigItem is T item && configureColumnItems != null)
            {
                var oldIndex = configureColumnItems.IndexOf(item);
                if (oldIndex > 0)
                {
                    configureColumnItems.Move(oldIndex, oldIndex - 1);
                    MoveCommandsRaiseCanExcuteChanged();
                }
            }
        }

        private bool CanMoveSelectedItemToPreviousIndex()
        {
            if (SelectedConfigItem is T item && configureColumnItems != null)
            {
                return configureColumnItems.IndexOf(item) > 0;
            }
            return false;
        }

        private void MoveSelectedItemToNextIndex()
        {
            if (SelectedConfigItem is T item && configureColumnItems != null)
            {
                var oldIndex = configureColumnItems.IndexOf(item);
                if (oldIndex < (configureColumnItems.Count - 1))
                {
                    configureColumnItems.Move(oldIndex, oldIndex + 1);
                    MoveCommandsRaiseCanExcuteChanged();
                }
            }
        }

        private bool CanMoveSelectedItemToNextIndex()
        {
            if (SelectedConfigItem is T item && configureColumnItems != null)
            {
                return configureColumnItems.IndexOf(item) < (configureColumnItems.Count - 1);
            }
            return false;
        }

        private void MoveSelectedItemToFirstIndex()
        {
            if (SelectedConfigItem is T item && configureColumnItems != null)
            {
                var oldIndex = configureColumnItems.IndexOf(item);
                if (oldIndex > 0)
                {
                    configureColumnItems.Move(oldIndex, 0);
                    MoveCommandsRaiseCanExcuteChanged();
                }
            }
        }

        private bool CanMoveSelectedItemToFirstIndex()
        {
            if (SelectedConfigItem is T item && configureColumnItems != null)
            {
                return configureColumnItems.IndexOf(item) > 0;
            }
            return false;
        }

        private void MoveSelectedItemToLastIndex()
        {
            if (SelectedConfigItem is T item && configureColumnItems != null)
            {
                var oldIndex = configureColumnItems.IndexOf(item);
                if (oldIndex < (configureColumnItems.Count - 1))
                {
                    configureColumnItems.Move(oldIndex, configureColumnItems.Count - 1);
                    MoveCommandsRaiseCanExcuteChanged();
                }
            }
        }

        private bool CanMoveSelectedItemToLastIndex()
        {
            if (SelectedConfigItem is T item && configureColumnItems != null)
            {
                return configureColumnItems.IndexOf(item) < (configureColumnItems.Count - 1);
            }
            return false;
        }

        private void MoveCommandsRaiseCanExcuteChanged()
        {
            moveSelectedItemToPreviousIndexCmd.RaiseCanExecuteChanged();
            moveSelectedItemToNextIndexCmd.RaiseCanExecuteChanged();
            moveSelectedItemToFirstIndexCmd.RaiseCanExecuteChanged();
            moveSelectedItemToLastIndexCmd.RaiseCanExecuteChanged();
        }
    }
}
