using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class PathHelper
    {
        /// <summary>
        /// 应用的安装目录
        /// </summary>
        public readonly static string AppSetupDirectoryPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
    }
}
