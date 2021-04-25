using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Data;
using System.Windows.Input;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Applications.DataModels;
using XueQiaoWaf.Trade.Modules.Applications.Services;
using XueQiaoWaf.Trade.Modules.Applications.Views;

namespace XueQiaoWaf.Trade.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class XQComposeOrderEPTManVM : ViewModel<IXQComposeOrderEPTManView>
    {
        private XQComposeOrderEPTService _XQComposeOrderEPTService;

        [ImportingConstructor]
        protected XQComposeOrderEPTManVM(IXQComposeOrderEPTManView view,
            XQComposeOrderEPTService XQComposeOrderEPTService) : base(view)
        {
            EnumHelper.GetAllTypesForEnum(typeof(XQComposeOrderExecParamsSendType), out IEnumerable<XQComposeOrderExecParamsSendType> allTypes);
            this._XQComposeOrderEPTTypes = allTypes;

            this._XQComposeOrderEPTService = XQComposeOrderEPTService;
            this.EPTs = new SynchronizingCollection<XQComposeOrderExecParamsTemplate, XQComposeOrderExecParamsTemplate>
                (XQComposeOrderEPTService.EPTs, i => i);
        }

        private readonly IEnumerable<XQComposeOrderExecParamsSendType> _XQComposeOrderEPTTypes;
        /// <summary>
        /// 获取所有执行参数模板类型
        /// </summary>
        public XQComposeOrderExecParamsSendType[] XQComposeOrderEPTTypes
        {
            get
            {
                return _XQComposeOrderEPTTypes.ToArray();
            }
        }

        /// <summary>
        /// 模板列表
        /// </summary>
        public ReadOnlyObservableCollection<XQComposeOrderExecParamsTemplate> EPTs { get; private set; }

        private XQComposeOrderExecParamsTemplate activedTemplate;
        /// <summary>
        /// 当前显示的模板
        /// </summary>
        public XQComposeOrderExecParamsTemplate ActivedTemplate
        {
            get { return activedTemplate; }
            set { SetProperty(ref activedTemplate, value); }
        }

        private ICommand newTemplateCmd;
        /// <summary>
        /// 新建模板 command
        /// </summary>
        public ICommand NewTemplateCmd
        {
            get { return newTemplateCmd; }
            set { SetProperty(ref newTemplateCmd, value); }
        }

        private ICommand templateParamsSetDefaultCmd;
        public ICommand TemplateParamsSetDefaultCmd
        {
            get { return templateParamsSetDefaultCmd; }
            set { SetProperty(ref templateParamsSetDefaultCmd, value); }
        }

        private ICommand templateSaveEditCmd;
        public ICommand TemplateSaveEditCmd
        {
            get { return templateSaveEditCmd; }
            set { SetProperty(ref templateSaveEditCmd, value); }
        }

        private ICommand templateCancelEditCmd;
        public ICommand TemplateCancelEditCmd
        {
            get { return templateCancelEditCmd; }
            set { SetProperty(ref templateCancelEditCmd, value); }
        }

        private ICommand templateOpenEditCmd;
        public ICommand TemplateOpenEditCmd
        {
            get { return templateOpenEditCmd; }
            set { SetProperty(ref templateOpenEditCmd, value); }
        }
        
        private ICommand templateRemoveCmd;
        public ICommand TemplateRemoveCmd
        {
            get { return templateRemoveCmd; }
            set { SetProperty(ref templateRemoveCmd, value); }
        }
        
    }
}
