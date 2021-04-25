using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Services
{
    /// <summary>
    /// 雪橇组合执行订单参数模板服务
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class XQComposeOrderEPTService
    {
        public XQComposeOrderEPTService()
        {
            EPTs = new ObservableCollection<XQComposeOrderExecParamsTemplate>();
            ArchivedEPTs = new ObservableCollection<XQComposeOrderExecParamsTemplate>();
        }

        /// <summary>
        /// 执行参数模板 datamodel 列表
        /// </summary>
        public ObservableCollection<XQComposeOrderExecParamsTemplate> EPTs { get; private set; }
        
        /// <summary>
        /// 执行参数模板已归档的模板列表
        /// </summary>
        public ObservableCollection<XQComposeOrderExecParamsTemplate> ArchivedEPTs { get; private set; }
    }
}
