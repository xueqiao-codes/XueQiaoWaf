using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Manage.Applications.DataModels
{
    /// <summary>
    /// 向导步骤 data model
    /// </summary>
    public class WizardStepItem : Model
    {
        public WizardStepItem(int stepCount)
        {
            this.StepCount = stepCount;
        }

        /// <summary>
        /// 总步骤数目
        /// </summary>
        public int StepCount { get; private set; }

        private int currentStepIndex;
        /// <summary>
        /// 当前步骤所在位置
        /// </summary>
        public int CurrentStepIndex
        {
            get { return currentStepIndex; }
            set
            {
                var tmp = value;
                if (tmp >= StepCount) tmp = (StepCount - 1);
                if (tmp < 0) tmp = 0;
                SetProperty(ref currentStepIndex, tmp);
            }
        }
    }
}
