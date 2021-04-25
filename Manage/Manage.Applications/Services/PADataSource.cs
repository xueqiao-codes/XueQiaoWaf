using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Models;

namespace Manage.Applications.Services
{
    /// <summary>
    /// 持仓预分配(Position Preview Assign)数据源
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    internal class PADataSource
    {
        private readonly List<PPAItem> previewAssignItems = new List<PPAItem>();
        private readonly object previewAssignItemsLock = new object();
        
        public PADataSource()
        {

        }

        /// <summary>
        /// 线程安全操作持仓预分配列表
        /// </summary>
        /// <param name="operationBlock"></param>
        public void SafelyOperationPreviewAssignItems(Action<IList<PPAItem>> operationBlock)
        {
            lock (previewAssignItemsLock)
            {
                operationBlock?.Invoke(previewAssignItems);
            }
        }
    }
}
