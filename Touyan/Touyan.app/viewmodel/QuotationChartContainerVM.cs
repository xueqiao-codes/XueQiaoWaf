using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using Touyan.app.view;
using XueQiaoFoundation.Shared.Model;

namespace Touyan.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class QuotationChartContainerVM : ViewModel<QuotationChartContainerView>
    {
        [ImportingConstructor]
        public QuotationChartContainerVM(QuotationChartContainerView view) : base(view)
        {
            TabItems = new ObservableCollection<SimpleTabItem>();
        }

        public ObservableCollection<SimpleTabItem> TabItems { get; }

        private SimpleTabItem selectedTabItem;
        public SimpleTabItem SelectedTabItem
        {
            get { return selectedTabItem; }
            set { SetProperty(ref selectedTabItem, value); }
        }

        private object chartDetailContentView;
        public object ChartDetailContentView
        {
            get { return chartDetailContentView; }
            set { SetProperty(ref chartDetailContentView, value); }
        }
    }
}
