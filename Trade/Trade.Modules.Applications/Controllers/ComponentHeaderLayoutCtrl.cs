using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Interface;
using XueQiaoWaf.Trade.Modules.Applications.ViewModels;

namespace XueQiaoWaf.Trade.Modules.Applications.Controllers
{
    /// <summary>
    /// 点击设置按钮处理器
    /// </summary>
    /// <param name="headerCtrl"></param>
    /// <param name="triggerElement">触发UI元素</param>
    internal delegate void ClickComponentSettingHandler(ComponentHeaderLayoutCtrl headerCtrl, object triggerElement);

    internal delegate void ClickComponentCloseHandler(ComponentHeaderLayoutCtrl headerCtrl);

    /// <summary>
    /// 组件头部视图控制器
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ComponentHeaderLayoutCtrl : IController
    {
        private readonly DelegateCommand closeComponentCommand;
        private readonly DelegateCommand triggerSettingCommand;
        private readonly DelegateCommand lockComponentOrNotCmd;

        [ImportingConstructor]
        public ComponentHeaderLayoutCtrl(ComponentHeaderLayoutVM headerLayoutVM)
        {
            this.HeaderLayoutVM = headerLayoutVM;

            closeComponentCommand = new DelegateCommand(() => ClickComponentClose?.Invoke(this));
            triggerSettingCommand = new DelegateCommand((param) => ClickComponentSetting?.Invoke(this, param));
            lockComponentOrNotCmd = new DelegateCommand(LockComponentOrNot);
        }

        public XueQiaoFoundation.BusinessResources.DataModels.TradeComponent Component { get; set; }

        /// <summary>
        /// 组件关闭处理器
        /// </summary>
        public ClickComponentCloseHandler ClickComponentClose { get; set; }

        /// <summary>
        /// 组件设置处理器
        /// </summary>
        public ClickComponentSettingHandler ClickComponentSetting { get; set; }

        public ComponentHeaderLayoutVM HeaderLayoutVM { get; private set; }

        public void Initialize()
        {
            if (Component == null) throw new ArgumentNullException("Component");

            HeaderLayoutVM.CloseComponentCommand = closeComponentCommand;
            HeaderLayoutVM.TriggerSettingCommand = triggerSettingCommand;
            HeaderLayoutVM.LockComponentOrNotCommand = lockComponentOrNotCmd;

            HeaderLayoutVM.IsComponentLocked = Component.IsLocked;            
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            ClickComponentClose = null;
            ClickComponentSetting = null;
        }
        
        private void LockComponentOrNot()
        {
            var tarLocked = !HeaderLayoutVM.IsComponentLocked;

            HeaderLayoutVM.IsComponentLocked = tarLocked;
            Component.IsLocked = tarLocked;
        }
    }
}
