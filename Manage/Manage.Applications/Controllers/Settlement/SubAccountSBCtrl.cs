using Manage.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Interface;

namespace Manage.Applications.Controllers
{
    /// <summary>
    /// 操作账号结算单页面 controller
    /// </summary>
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SubAccountSBCtrl : IController
    {
        private readonly SubAccountSettlementContainerVM contentVM;

        [ImportingConstructor]
        public SubAccountSBCtrl(SubAccountSettlementContainerVM contentVM)
        {
            this.contentVM = contentVM;
        }

        public object ContentView => contentVM.View;

        public void Initialize()
        {
           
        }

        public void Run()
        {

        }

        public void Shutdown()
        {

        }
    }
}
