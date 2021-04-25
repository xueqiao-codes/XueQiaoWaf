using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace XueQiaoFoundation.UI.Components.Popup
{
    public interface IPopup
    {
        UIElement PlacementTarget { get; set; }

        double VerticalOffset { get; set; }

        double HorizontalOffset { get; set; }

        bool StaysOpen { get; set; }

        PlacementMode Placement { get; set; }

        /// <summary>
        /// 弹层的内容
        /// </summary>
        UIElement Content { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        bool IsOpen { get; }

        event EventHandler Closed;

        event EventHandler Opened;
        
        void Open();

        void Close();
    }
}
