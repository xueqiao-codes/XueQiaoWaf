using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using Touyan.app.datamodel;
using Touyan.app.view;

namespace Touyan.app.viewmodel
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ChartFolderContainerVM : ViewModel<ChartFolderContainerView>
    {
        [ImportingConstructor]
        public ChartFolderContainerVM(ChartFolderContainerView view) : base(view)
        {
            TopFolderTabItems = new ObservableCollection<TopChartFolderTabItem>();
        }
        
        public ObservableCollection<TopChartFolderTabItem> TopFolderTabItems { get; private set; }

        private TopChartFolderTabItem selectedTopFolderTabItem;
        public TopChartFolderTabItem SelectedTopFolderTabItem
        {
            get { return selectedTopFolderTabItem; }
            set { SetProperty(ref selectedTopFolderTabItem, value); }
        }
    }
}
