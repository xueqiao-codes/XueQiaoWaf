using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xueqiao.graph.xiaoha.chart.thriftapi;

namespace Touyan.app.datamodel
{
    /// <summary>
    /// 图表文件夹列表树 Folder 类型项
    /// </summary>
    public class ChartFolderListTreeNode_Folder : ChartFolderListTreeNodeBase
    {
        public ChartFolderListTreeNode_Folder(long folderId, long parentFolderId) : base(parentFolderId)
        {
            this.FolderId = folderId;
            Children = new ObservableCollection<ChartFolderListTreeNodeBase>();
        }

        public long FolderId { get; private set; }
        
        public ObservableCollection<ChartFolderListTreeNodeBase> Children { get; private set; }
        
        private ChartFolder folder;
        public ChartFolder Folder
        {
            get { return folder; }
            set { SetProperty(ref folder, value); }
        }
        
        /// <summary>
        /// 是否正在加载子 node
        /// </summary>
        private bool isLoadingChildren;
        public bool IsLoadingChildren
        {
            get { return isLoadingChildren; }
            set { SetProperty(ref isLoadingChildren, value); }
        }

    }
}
