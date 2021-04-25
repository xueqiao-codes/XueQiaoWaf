using Manage.Applications.Views;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;

namespace Manage.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubAccountInOutFundDialogContentViewModel : ViewModel<ISubAccountInOutFundDialogContentView>
    {
        [ImportingConstructor]
        protected SubAccountInOutFundDialogContentViewModel(ISubAccountInOutFundDialogContentView view) : base(view)
        {
            CurrencyItems = ClientFundCurrencyReference.AllClientFundCurrencyList.ToArray();
        }

        public object DisplayInWindow => ViewCore.DisplayInWindow;

        public void CloseDisplayInWindow()
        {
            ViewCore.CloseDisplayInWindow();
        }

        public string[] CurrencyItems { get; private set; }

        private string selectedCurrency;
        /// <summary>
        /// 选中的币种
        /// </summary>
        public string SelectedCurrency
        {
            get { return selectedCurrency; }
            set { SetProperty(ref selectedCurrency, value); }
        }
        
        private bool isInFund;
        /// <summary>
        /// 是否为入金
        /// </summary>
        public bool IsInFund
        {
            get { return isInFund; }
            set { SetProperty(ref isInFund, value); }
        }
        
        private double inOrOutFund;
        /// <summary>
        /// 出入金金额
        /// </summary>
        public double InOrOutFund
        {
            get { return inOrOutFund; }
            set { SetProperty(ref inOrOutFund, value); }
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
