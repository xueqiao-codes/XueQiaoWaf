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
    public class ChartFolderSubItemListVM : ViewModel<ChartFolderSubItemListView>
    {
        [ImportingConstructor]
        public ChartFolderSubItemListVM(ChartFolderSubItemListView view) : base(view)
        {
            view.NodeItemSelectedHandler = item => NodeItemSelectedHandler(item as ChartFolderListTreeNodeBase);
            view.NodeItemExpandedHandler = item => NodeItemExpandedHandler(item as ChartFolderListTreeNodeBase);

            ChartFolderNodeTree = new ObservableCollection<ChartFolderListTreeNodeBase>();
        }

        public ObservableCollection<ChartFolderListTreeNodeBase> ChartFolderNodeTree { get; private set; }

        /// <summary>
        /// Node 选中的处理方法 
        /// </summary>
        public Action<ChartFolderListTreeNodeBase> NodeItemSelectedHandler { get; set; }

        /// <summary>
        /// Node 展开的处理方法 
        /// </summary>
        public Action<ChartFolderListTreeNodeBase> NodeItemExpandedHandler { get; set; }
    }
}
