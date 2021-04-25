using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.trade.hosting;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 新建操作账户向导数据实体
    /// </summary>
    public class SubAccountAddWizardData : Model
    {
        private SubAccountAddWizardStepData_SetAccountName stepData_SetAccountName;
        public SubAccountAddWizardStepData_SetAccountName StepData_SetAccountName
        {
            get { return stepData_SetAccountName; }
            set { SetProperty(ref stepData_SetAccountName, value); }
        }

        private SubAccountAddWizardStepData_SetInitialInFund stepData_setInitialInFund;
        public SubAccountAddWizardStepData_SetInitialInFund StepData_setInitialInFund
        {
            get { return stepData_setInitialInFund; }
            set { SetProperty(ref stepData_setInitialInFund, value); }
        }

        private SubAccountAddWizardStepData_AuthToSubUsers stepData_AuthToSubUsers;
        public SubAccountAddWizardStepData_AuthToSubUsers StepData_AuthToSubUsers
        {
            get { return stepData_AuthToSubUsers; }
            set { SetProperty(ref stepData_AuthToSubUsers, value); }
        }
    }

    /// <summary>
    /// 步骤[设置账户名]的数据
    /// </summary>
    public class SubAccountAddWizardStepData_SetAccountName : Model
    {
        /// <summary>
        /// 账户名称
        /// </summary>
        private string subAccountName;
        public string SubAccountName
        {
            get { return subAccountName; }
            set { SetProperty(ref subAccountName, value); }
        }
    }

    /// <summary>
    /// 步骤[设置初始入金]的数据
    /// </summary>
    public class SubAccountAddWizardStepData_SetInitialInFund : Model
    {
        private double initialInFund;
        /// <summary>
        /// 初始入金
        /// </summary>
        public double InitialInFund
        {
            get { return initialInFund; }
            set { SetProperty(ref initialInFund, value); }
        }
        
        private string currency;
        /// <summary>
        /// 币种
        /// </summary>
        public string Currency
        {
            get { return currency; }
            set { SetProperty(ref currency, value); }
        }
    }

    /// <summary>
    /// 步骤[分配用户]的数据
    /// </summary>
    public class SubAccountAddWizardStepData_AuthToSubUsers
    {
        /// <summary>
        /// 已分配的用户
        /// </summary>
        public ReadOnlyObservableCollection<SubUserSelectModel> AuthedToSubUsers { get; private set; }
    }

    /// <summary>
    /// 操作账户添加向导结果
    /// </summary>
    public struct SubAccountAddWizardResultReference
    {
        public const Int32 Canceled = 0;
        public const Int32 Finished = 1;
    }
}
