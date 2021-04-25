using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.DataModels
{
    /// <summary>
    /// 雪橇标的信息 data model 协议
    /// </summary>
    public interface IXqTargetDM : INotifyPropertyChanged
    {
        /// <summary>
        /// 雪橇标的类型
        /// </summary>
        ClientXQOrderTargetType TargetType { get; }
        
        /// <summary>
        /// 合约标的的容器
        /// </summary>
        TargetContract_TargetContractDetail TargetContractDetailContainer { get; }
        
        /// <summary>
        /// 组合标的的组合详情
        /// </summary>
        TargetCompose_ComposeDetail TargetComposeDetailContainer { get; }
        
        /// <summary>
        /// 组合标的的用户组合视图容器
        /// </summary>
        UserComposeViewContainer TargetComposeUserComposeViewContainer { get; }

        /// <summary>
        /// 雪橇标的名称。根据其他数据设置而来的，避免在界面中拼接影响性能
        /// </summary>
        string TargetName { get; set; }
    }
}
