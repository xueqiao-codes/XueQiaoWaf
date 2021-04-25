using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace Touyan.app.datamodel
{
    /// <summary>
    /// 图表文件夹树列表项 data model base
    /// </summary>
    public class ChartFolderListTreeNodeBase : Model
    {
        public ChartFolderListTreeNodeBase(long parentFolderId)
        {
            this.ParentFolderId = parentFolderId;
        }

        public long ParentFolderId { get; private set; }
    }
}
