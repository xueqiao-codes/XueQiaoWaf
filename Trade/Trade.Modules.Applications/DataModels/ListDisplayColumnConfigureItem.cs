using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using XueQiaoFoundation.BusinessResources.Models;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class ListDisplayColumnConfigureItem<T> : Model
    {
        public ListDisplayColumnConfigureItem(T column)
        {
            this.Column = column;
        }

        public T Column { get; private set; }

        private bool isToDisplay;
        /// <summary>
        /// 是否要进行显示
        /// </summary>
        public bool IsToDisplay
        {
            get { return isToDisplay; }
            set { SetProperty(ref isToDisplay, value); }
        }
    }

    public class ListDisplayColumnConfigureItem : ListDisplayColumnConfigureItem<ListColumnInfo>
    {
        public ListDisplayColumnConfigureItem(ListColumnInfo column) : base(column)
        {
        }
    }
}
