using System;
using System.Collections.Generic;
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
    public class EditComposeViewPrecisionNumberVM : ViewModel<IEditComposeViewPrecisionNumberView>
    {
        [ImportingConstructor]
        public EditComposeViewPrecisionNumberVM(IEditComposeViewPrecisionNumberView view) : base(view)
        {
        }

        private UserComposeViewContainer editComposeViewContainer;
        /// <summary>
        /// 要修改小数位数的组合视图容器
        /// </summary>
        public UserComposeViewContainer EditComposeViewContainer
        {
            get { return editComposeViewContainer; }
            set { SetProperty(ref editComposeViewContainer, value); }
        }

        private short precisionNumber;
        public short PrecisionNumber
        {
            get { return precisionNumber; }
            set { SetProperty(ref precisionNumber, value); }
        }
        
        private short precisionNumberMin;
        public short PrecisionNumberMin
        {
            get { return precisionNumberMin; }
            set { SetProperty(ref precisionNumberMin, value); }
        }

        private short precisionNumberMax;
        public short PrecisionNumberMax
        {
            get { return precisionNumberMax; }
            set { SetProperty(ref precisionNumberMax, value); }
        }

        private ICommand okCommand;
        public ICommand OkCommand
        {
            get { return okCommand; }
            set { SetProperty(ref okCommand, value); }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { SetProperty(ref cancelCommand, value); }
        }
    }
}
