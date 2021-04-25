using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.Models;
using XueQiaoFoundation.Shared.Model;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 投研组件
    /// </summary>
    public class ResearchComponent : XQComponentBase
    {
        public ResearchComponent()
        {
            UrlCompDetail = new RS_UrlCompDetail();
        }

        private int componentType;
        public int ComponentType
        {
            get { return componentType; }
            set { SetProperty(ref componentType, value); }
        }

        /// <summary>
        /// 投研 url 组件详情
        /// </summary>
        public RS_UrlCompDetail UrlCompDetail { get; set; }
    }
}
