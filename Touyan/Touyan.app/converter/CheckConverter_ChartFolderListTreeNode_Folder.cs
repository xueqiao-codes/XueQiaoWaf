using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Touyan.app.datamodel;

namespace Touyan.app.converter
{
    /// <summary>
    /// 检查是否为<see cref="CheckConverter_ChartFolderListTreeNode_Folder"/>类型
    /// </summary>
    public class CheckConverter_ChartFolderListTreeNode_Folder : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is ChartFolderListTreeNode_Folder;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
