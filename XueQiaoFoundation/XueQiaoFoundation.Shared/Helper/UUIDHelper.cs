using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    /// <summary>
    /// uuid 辅助
    /// </summary>
    public static class UUIDHelper
    {
        public static string CreateUUIDString(bool removeSeperator)
        {
            var uuid = Guid.NewGuid().ToString();
            if (removeSeperator)
            {
                uuid = uuid.Replace("-", "");
            }
            return uuid;
        }
    }
}
