using Manage.Applications.Controllers;
using Manage.Interfaces.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Applications
{
    [Export(typeof(IModuleController)), Export(typeof(IPersonalUserManageModuleService)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class PersonalUserManageModuleController : IModuleController, IPersonalUserManageModuleService
    {
        private readonly ExportFactory<PersonalUserManageModuleRootViewCtrl> PUManageModuleRootViewCtrlFactory;

        private PersonalUserManageModuleRootViewCtrl _PUManageModuleRootViewCtrl;
        private bool shutdowned;

        [ImportingConstructor]
        public PersonalUserManageModuleController(ExportFactory<PersonalUserManageModuleRootViewCtrl> PUManageModuleRootViewCtrlFactory)
        {
            this.PUManageModuleRootViewCtrlFactory = PUManageModuleRootViewCtrlFactory;
        }

        public void Initialize()
        {
            
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            if (this.shutdowned) return;
            this.shutdowned = true;

            _PUManageModuleRootViewCtrl?.Shutdown();
            _PUManageModuleRootViewCtrl = null;
            
        }

        public object GetPersonalUserManageModuleRootView(Func<ChromeWindowCaptionDataHolder> embedInWindowCaptionDataHolderGetter,
            out Action showAction, out Action closeAction)
        {
            showAction = null;
            closeAction = null;

            if (this._PUManageModuleRootViewCtrl != null)
                return this._PUManageModuleRootViewCtrl.ContentView;

            this._PUManageModuleRootViewCtrl = PUManageModuleRootViewCtrlFactory.CreateExport().Value;
            this._PUManageModuleRootViewCtrl.EmbedInWindowCaptionDataHolder = embedInWindowCaptionDataHolderGetter?.Invoke();

            this._PUManageModuleRootViewCtrl.Initialize();
            this._PUManageModuleRootViewCtrl.Run();

            return this._PUManageModuleRootViewCtrl.ContentView;
        }
    }
}
