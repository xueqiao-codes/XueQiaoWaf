using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 工作区窗口
    /// </summary>
    public class TabWorkspaceWindow : Model
    {
        /// <summary>
        /// 所在坐标 left
        /// </summary>
        private double left;
        public double Left
        {
            get { return left; }
            set { SetProperty(ref left, value); }
        }

        /// <summary>
        /// 所在坐标 top
        /// </summary>
        private double top;
        public double Top
        {
            get { return top; }
            set { SetProperty(ref top, value); }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        private double width;
        public double Width
        {
            get { return width; }
            set { SetProperty(ref width, value); }
        }

        /// <summary>
        /// 高度
        /// </summary>
        private double height;
        public double Height
        {
            get { return height; }
            set { SetProperty(ref height, value); }
        }

        /// <summary>
        /// 是否最大化
        /// </summary>
        private bool isMaximized;
        public bool IsMaximized
        {
            get { return isMaximized; }
            set { SetProperty(ref isMaximized, value); }
        }
    }
}
