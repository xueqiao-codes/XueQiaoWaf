using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.Shared.Model
{
    public class XQComponentBase : System.Waf.Foundation.Model
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
        /// 组件的 Zindex。组件叠放在一起时，ZIndex大的显示在上层。
        /// </summary>
        private int zIndex;
        public int ZIndex
        {
            get { return zIndex; }
            set { SetProperty(ref zIndex, value); }
        }
    }
}
