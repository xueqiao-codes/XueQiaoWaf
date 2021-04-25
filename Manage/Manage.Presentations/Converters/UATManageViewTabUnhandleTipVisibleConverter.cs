using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Manage.Presentations.Converters
{
    /// <summary>
    /// 未分配成交管理页面的 tab 未处理标志是否显示的转换器
    /// </summary>
    public class UATManageViewTabUnhandleTipVisibleConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换方法。 
        /// values的值:
        /// values[0]: tab 类型，<see cref="UATManageViewTabType"/>
        /// values[1]: 未预分配的总量。int 类型
        /// values[2]: 已预分配（待提交）的总量。int 类型
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value0 = values[0] as UATManageViewTabType?;
            var value1 = values[1] as int?;
            var value2 = values[2] as int?;

            if (value0 == null || value1 == null || value2 == null)
                return Visibility.Collapsed;

            if (value0 == UATManageViewTabType.AssignTab)
            {
                return value1 > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value0 == UATManageViewTabType.PreviewTab)
            {
                return value2 > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
