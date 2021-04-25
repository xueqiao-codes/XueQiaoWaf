using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.BusinessResources.DataModels;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoFoundation.Shared.Model;

namespace Research.app.Controller
{
    internal interface IResearchComponentCtrl : IController
    {
        /// <summary>
        /// 交易组件所属父工作区
        /// </summary>
        TabWorkspace ParentWorkspace { get; }

        /// <summary>
        /// 投研组件
        /// </summary>
        ResearchComponent Component { get; }

        /// <summary>
        /// 组件 controller 关闭处理器
        /// </summary>
        Action<IResearchComponentCtrl> CloseComponentHandler { get; }

        /// <summary>
        /// 组件 data model
        /// </summary>
        DraggableComponentUIDM ComponentItemDataModel { get; }
    }
}
