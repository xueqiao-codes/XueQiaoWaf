using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using Touyan.app.controller;

namespace Touyan.app
{
    [Export(typeof(IModuleController)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class TouyanModuleController : IModuleController
    {
        private readonly CompositionContainer compositionContainer;
        private readonly ExportFactory<TouyanModuleRootViewCtrl> moduleRootViewCtrlFactory;

        private IEnumerable<ITouyanModuleServiceCtrl> moduleServiceCtrls;
        private TouyanModuleRootViewCtrl moduleRootViewCtrl;

        private bool shutdowned;

        [ImportingConstructor]
        public TouyanModuleController(
            CompositionContainer compositionContainer,
            ExportFactory<TouyanModuleRootViewCtrl> moduleRootViewCtrlFactory)
        {
            this.compositionContainer = compositionContainer;
            this.moduleRootViewCtrlFactory = moduleRootViewCtrlFactory;
        }

        public void Initialize()
        {
            
        }

        public void Run()
        {
            moduleServiceCtrls = compositionContainer.GetExportedValues<ITouyanModuleServiceCtrl>()?.ToArray();
            if (moduleServiceCtrls?.Any() == true)
            {
                foreach (var ctrl in moduleServiceCtrls)
                {
                    ctrl.Initialize();
                }
            }
        }

        public void Shutdown()
        {
            if (this.shutdowned) return;
            this.shutdowned = true;

            moduleRootViewCtrl?.Shutdown();
            moduleRootViewCtrl = null;

            if (moduleServiceCtrls?.Any() == true)
            {
                foreach (var ctrl in moduleServiceCtrls)
                {
                    ctrl.Shutdown();
                }
            }
        }
    }
}
