using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace XueQiaoFoundation.UI.Extra.helper
{
    /// <summary>
    /// 编辑框双击选中所有字符行为
    /// </summary>
    public class TextBoxDoubleClickSelectAllTextBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDoubleClick += AssociatedObject_MouseDoubleClick;
        }

        private void AssociatedObject_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AssociatedObject?.SelectAll();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseDoubleClick -= AssociatedObject_MouseDoubleClick;
        }
    }
}
