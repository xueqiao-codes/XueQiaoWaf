using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 雪橇标的 data model
    /// </summary>
    public class XqTargetDM : Model, IXqTargetDM
    {
        public XqTargetDM(ClientXQOrderTargetType targetType)
        {
            this.TargetType = targetType;
        }

        /// <summary>
        /// 雪橇标的类型
        /// </summary>
        public ClientXQOrderTargetType TargetType { get; private set; }
        
        private TargetContract_TargetContractDetail targetContractDetailContainer;
        /// <summary>
        /// 合约标的的容器
        /// </summary>
        public TargetContract_TargetContractDetail TargetContractDetailContainer
        {
            get { return targetContractDetailContainer; }
            set { SetProperty(ref targetContractDetailContainer, value); }
        }
        
        private TargetCompose_ComposeDetail targetComposeDetailContainer;
        /// <summary>
        /// 组合标的的组合详情
        /// </summary>
        public TargetCompose_ComposeDetail TargetComposeDetailContainer
        {
            get { return targetComposeDetailContainer; }
            set { SetProperty(ref targetComposeDetailContainer, value); }
        }

        private UserComposeViewContainer targetComposeUserComposeViewContainer;
        /// <summary>
        /// 组合标的的用户组合视图容器
        /// </summary>
        public UserComposeViewContainer TargetComposeUserComposeViewContainer
        {
            get { return targetComposeUserComposeViewContainer; }
            set { SetProperty(ref targetComposeUserComposeViewContainer, value); }
        }

        private string targetName;
        /// <summary>
        /// 雪橇标的名称。根据其他数据设置而来的，避免在界面中拼接影响性能
        /// </summary>
        public string TargetName
        {
            get { return targetName; }
            set { SetProperty(ref targetName, value); }
        }
    }
}
