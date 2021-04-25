using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TradeWorkspaceTemplateManageDialogContentVM : ViewModel<ITradeWorkspaceTemplateManageDialogContentView>
    {
        [ImportingConstructor]
        protected TradeWorkspaceTemplateManageDialogContentVM(ITradeWorkspaceTemplateManageDialogContentView view) : base(view)
        {
        }


        private ReadOnlyObservableCollection<TradeTabWorkspaceTemplate> tradeWorkspaceTemplates;
        /// <summary>
        /// 交易工作区模板列表
        /// </summary>
        public ReadOnlyObservableCollection<TradeTabWorkspaceTemplate> TradeWorkspaceTemplates
        {
            get { return tradeWorkspaceTemplates; }
            set { SetProperty(ref tradeWorkspaceTemplates, value); }
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

        private ICommand toDeleteItemCmd;
        public ICommand ToDeleteItemCmd
        {
            get { return toDeleteItemCmd; }
            set { SetProperty(ref toDeleteItemCmd, value); }
        }

        private ICommand toEditItemCmd;
        public ICommand ToEditItemCmd
        {
            get { return toEditItemCmd; }
            set { SetProperty(ref toEditItemCmd, value); }
        }
    }
}
