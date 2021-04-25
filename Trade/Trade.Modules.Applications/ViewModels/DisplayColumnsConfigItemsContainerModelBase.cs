using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.BusinessResources.Constants;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    public class DisplayColumnsConfigItemsContainerModelBase<TView> : ViewModel<TView> where TView : IView
    {
        protected DisplayColumnsConfigItemsContainerModelBase(TView view) : base(view)
        {
            ColumnConfigItems = new ObservableCollection<ListDisplayColumnConfigureItem>();
        }

        private ListDisplayColumnConfigureItem selectedConfigItem;
        public ListDisplayColumnConfigureItem SelectedConfigItem
        {
            get { return selectedConfigItem; }
            set { SetProperty(ref selectedConfigItem, value); }
        }

        public ObservableCollection<ListDisplayColumnConfigureItem> ColumnConfigItems { get; private set; }

        /// <summary>
        /// 列对其方式列表。Item 类型<see cref="ListColumnContentAlignmentReference"/>
        /// </summary>
        public int[] ColumnContentAlignmentTypes
        {
            get { return XueQiaoConstants.AllListColumnContentAlignments.ToArray(); }
        }
    }
}
