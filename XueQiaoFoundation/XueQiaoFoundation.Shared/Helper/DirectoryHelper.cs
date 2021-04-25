using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public class DirectoryHelper
    {        
        public static DirectoryInfo CreateDirectoryIfNeed(string pathStr, out Exception exception)
        {
            exception = null;
            if (string.IsNullOrWhiteSpace(pathStr)) return null;
            try
            {
                DirectoryInfo di = new DirectoryInfo(pathStr);
                di.Create();
                return di;
            }
            catch (Exception e)
            {
                exception = e;
                return null;
            }
        }
    }
}
