using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class OrderOperateConfirmVM : ViewModel<IOrderOperateConfirmView>
    {
        [ImportingConstructor]
        public OrderOperateConfirmVM(IOrderOperateConfirmView view) : base(view)
        {
        }

        /// <summary>
        /// 需要确认的内容
        /// </summary>
        private object needConfirmMessage;
        public object NeedConfirmMessage
        {
            get { return needConfirmMessage; }
            set { SetProperty(ref needConfirmMessage, value); }
        }

        /// <summary>
        /// 下次是否不再提示确认
        /// </summary>
        private bool notConfirmNextTime;
        public bool NotConfirmNextTime
        {
            get { return notConfirmNextTime; }
            set { SetProperty(ref notConfirmNextTime, value); }
        }

        /// <summary>
        /// 视图内容的 margin
        /// </summary>
        private Thickness viewContentMargin;
        public Thickness ViewContentMargin
        {
            get { return viewContentMargin; }
            set { SetProperty(ref viewContentMargin, value); }
        }

    }
}
