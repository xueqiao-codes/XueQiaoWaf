using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Waf.Applications;
using XueQiaoFoundation.Interfaces.Applications;
using XueQiaoWaf.LoginUserManage.Interfaces.Applications;

namespace XueQiaoFoundation.Applications
{
    [Export(typeof(IModuleController)), Export(typeof(IExchangeDataService)), PartCreationPolicy(CreationPolicy.Shared)]
    internal class ModuleController : IModuleController, IExchangeDataService
    {   
        private readonly CompositionContainer compositionContainer;

        private IEnumerable<IXueQiaoFoundationSingletonController> moduleSingletonCtrls;
        private bool shutdowned;
        
        [ImportingConstructor]
        public ModuleController(CompositionContainer compositionContainer,
            Lazy<ILoginDataService> loginDataService)
        {
            this.compositionContainer = compositionContainer;
        }
        
        public void Initialize()
        {
            moduleSingletonCtrls = compositionContainer.GetExportedValues<IXueQiaoFoundationSingletonController>().ToArray();
        }

        public void Run()
        {

        }

        public void Shutdown()
        {
            if (this.shutdowned) return;
            this.shutdowned = true;

            foreach (var ctrl in moduleSingletonCtrls)
            {
                ctrl.Shutdown();
            }
        }
        
        #region IExchangeDataService

        public bool IsInnerExchange(string sledExchangeMic)
        {
            return mInnerExchangeMics.Any(i => i.ToUpper() == sledExchangeMic.ToUpper());
        }

        public string[] InnerExchangeMicList => mInnerExchangeMics.ToArray();

        public string[] PreferredExchangeMicList => mPreferredExchangeMicList.ToArray();

        public string[] PreferredExchangeCountryAcronymList => mPreferredExchangeCountryAcronymList.ToArray();
        
        // 内盘交易所代码列表
        private static readonly string[] mInnerExchangeMics = new string[] {
            "CCFX", "XDCE", "XSGE", "XZCE", "XINE"
        };

        private static readonly string[] mPreferredExchangeMicList = new string[] {
        /* 中国 CN*/    "XSGE", "XDCE", "XZCE", "CCFX", "XINE", 
        /* 香港 HK*/    "XHKG",
        /* 美国 US*/    "XCBT", "XCME", "XNYM", "XCEC", "IFUS", "IEPA", "XCBO",
        /* 新加坡 SG*/  "XSES", "IFSG", 
        /* 英国 GB*/    "XLME", "IFEU",
        };

        private static readonly string[] mPreferredExchangeCountryAcronymList = new string[] {
            "CN","HK","US","SG","GB"
        };
        
        #endregion
    }
}
