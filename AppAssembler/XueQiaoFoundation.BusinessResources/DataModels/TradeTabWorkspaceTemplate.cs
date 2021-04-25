using System;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 交易工作空间模板，用于保存用户工作空间信息的承载
    /// </summary>
    public class TradeTabWorkspaceTemplate : Model
    {
        public TradeTabWorkspaceTemplate(string templateId)
        {
            if (string.IsNullOrEmpty(templateId)) throw new ArgumentException("`templateId` can't be empty.");
            this.TemplateId = templateId;
        }

        /// <summary>
        /// 工作空间的 id，请确保该值的唯一性，可使用uuid提供
        /// </summary>
        public string TemplateId { get; private set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        private string templateName;
        public string TemplateName
        {
            get { return templateName; }
            set { SetProperty(ref templateName, value); }
        }
        
        /// <summary>
        /// 描述
        /// </summary>
        private string templateDesc;
        public string TemplateDesc
        {
            get { return templateDesc; }
            set { SetProperty(ref templateDesc, value); }
        }


        private int createTimestamp;
        public int CreateTimestamp
        {
            get { return createTimestamp; }
            set { SetProperty(ref createTimestamp, value); }
        }

        private int lastUpdateTimestamp;
        public int LastUpdateTimestamp
        {
            get { return lastUpdateTimestamp; }
            set { SetProperty(ref lastUpdateTimestamp, value); }
        }
        
        /// <summary>
        /// 包含的组件
        /// </summary>
        public XueQiaoFoundation.BusinessResources.Models.TradeComp[] ChildComponents { get; set; }
    }
}
