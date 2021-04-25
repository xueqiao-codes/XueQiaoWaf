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
    public class ListDisplayColumnsConfigDialogContentViewModel<T> : ViewModel<IListDisplayColumnsConfigDialogContentView>
    {
        [ImportingConstructor]
        protected ListDisplayColumnsConfigDialogContentViewModel(IListDisplayColumnsConfigDialogContentView view) : base(view)
        {
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

        private ListDisplayColumnsConfigItemMoveCommands<T> configItemMoveCommands;
        public ListDisplayColumnsConfigItemMoveCommands<T> ConfigItemMoveCommands
        {
            get { return configItemMoveCommands; }
            set { SetProperty(ref configItemMoveCommands, value); }
        }
        
        private object displayColumnsConfigItemsContainerView;
        /// <summary>
        /// 配置项列表容器视图
        /// </summary>
        public object DisplayColumnsConfigItemsContainerView
        {
            get { return displayColumnsConfigItemsContainerView; }
            set { SetProperty(ref displayColumnsConfigItemsContainerView, value); }
        }

        private string applyAsGlobalText;
        /// <summary>
        /// 应用到全局的文案
        /// </summary>
        public string ApplyAsGlobalText
        {
            get { return applyAsGlobalText; }
            set { SetProperty(ref applyAsGlobalText, value); }
        }
        
        private bool isApplyAsGlobal;
        /// <summary>
        /// 是否应用到全局
        /// </summary>
        public bool IsApplyAsGlobal
        {
            get { return isApplyAsGlobal; }
            set { SetProperty(ref isApplyAsGlobal, value); }
        }

        private ICommand resetToDefaultDisplayColumnsCmd;
        public ICommand ResetToDefaultDisplayColumnsCmd
        {
            get { return resetToDefaultDisplayColumnsCmd; }
            set { SetProperty(ref resetToDefaultDisplayColumnsCmd, value); }
        }

        private ICommand saveCmd;
        public ICommand SaveCmd
        {
            get { return saveCmd; }
            set { SetProperty(ref saveCmd, value); }
        }

        private ICommand cancelCmd;
        public ICommand CancelCmd
        {
            get { return cancelCmd; }
            set { SetProperty(ref cancelCmd, value); }
        }
    }
}
