using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ComponentHeaderLayoutVM : ViewModel<IComponentHeaderLayoutView>
    {
        [ImportingConstructor]
        public ComponentHeaderLayoutVM(IComponentHeaderLayoutView view) : base(view)
        {
        }
        
        /// <summary>
        /// 是否显示关闭 item
        /// </summary>
        private bool showCloseItem;
        public bool ShowCloseItem
        {
            get { return showCloseItem; }
            set { SetProperty(ref showCloseItem, value); }
        }

        /// <summary>
        /// 是否显示设置 item
        /// </summary>
        private bool showSettingItem;
        public bool ShowSettingItem
        {
            get { return showSettingItem; }
            set { SetProperty(ref showSettingItem, value); }
        }
        
        /// <summary>
        /// 是否显示锁 item
        /// </summary>
        private bool showComponentLockItem;
        public bool ShowComponentLockItem
        {
            get { return showComponentLockItem; }
            set { SetProperty(ref showComponentLockItem, value); }
        }

        private bool isComponentLocked;
        public bool IsComponentLocked
        {
            get { return isComponentLocked; }
            set { SetProperty(ref isComponentLocked, value); }
        }
        
        private ICommand closeComponentCommand;
        /// <summary>
        /// 关闭组件 command
        /// </summary>
        public ICommand CloseComponentCommand
        {
            get { return closeComponentCommand; }
            set { SetProperty(ref closeComponentCommand, value); }
        }

        private ICommand triggerSettingCommand;
        /// <summary>
        /// 触发设置 command
        /// </summary>
        public ICommand TriggerSettingCommand
        {
            get { return triggerSettingCommand; }
            set { SetProperty(ref triggerSettingCommand, value); }
        }
        
        /// <summary>
        /// 锁定或解锁组件 command
        /// </summary>
        private ICommand lockComponentOrNotCommand;
        public ICommand LockComponentOrNotCommand
        {
            get { return lockComponentOrNotCommand; }
            set { SetProperty(ref lockComponentOrNotCommand, value); }
        }
        
        private object customPartView;
        public object CustomPartView
        {
            get { return customPartView; }
            set { SetProperty(ref customPartView, value); }
        }
    }
}
