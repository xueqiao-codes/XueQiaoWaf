using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Models
{
    /// <summary>
    /// 投研组件
    /// </summary>
    public class ResearchComp
    {

        // 位置 left
        public double Left { get; set; }

        // 位置 top
        public double Top { get; set; }

        // 大小 width
        public double Width { get; set; }

        // 大小 height
        public double Height { get; set; }

        /// <summary>
        /// 组件的 Zindex。组件叠放在一起时，ZIndex 大的显示在上层。
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// 投研组件的类型。参考 XueQiaoConstants 定义的`投研组件类型`
        /// </summary>
        public int ComponentType { get; set; }

        /// <summary>
        /// 投研 url 组件详情
        /// </summary>
        public RS_UrlCompDetail UrlCompDetail { get; set; }
    }
    
    /// <summary>
    /// 投研的 url 组件 
    /// </summary>
    public class RS_UrlCompDetail
    {
        public string Url { get; set; }
    }
    
}
