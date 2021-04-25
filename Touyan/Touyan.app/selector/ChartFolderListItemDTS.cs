using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Touyan.Interface.datamodel;

namespace Touyan.app.selector
{
    /// <summary>
    /// 图表文件夹列表项 data template selector
    /// </summary>
    public class ChartFolderListItemDTS : DataTemplateSelector
    {
        /// <summary>
        /// 文件夹类型项的 data template
        /// </summary>
        public DataTemplate DT_Folder { get; set; }

        /// <summary>
        /// 图表类型项的 data template
        /// </summary>
        public DataTemplate DT_Chart { get; set; }

        /// <summary>
        /// 选择模板方法
        /// </summary>
        /// <param name="item">必须为<see cref="ChartFolderListItemType"/>类型的数据</param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ChartFolderListItemType _itemType)
            {
                if (_itemType == ChartFolderListItemType.Folder)
                    return this.DT_Folder;
                else if (_itemType == ChartFolderListItemType.Chart)
                    return this.DT_Chart;
            }
            return null;
        }
    }
}
