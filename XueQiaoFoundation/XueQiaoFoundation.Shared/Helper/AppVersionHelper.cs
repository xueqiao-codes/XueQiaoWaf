using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public class AppVersionHelper
    {
        public static Version GetApplicationVersion()
        {
            try
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            catch
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                return entryAssembly.GetName().Version;
            }
        }
    }
}
