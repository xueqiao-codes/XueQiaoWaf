using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Helpers
{
    public enum VersionUpdatePolicy
    {
        NoUpdate = 1,           // 不需更新
        OptionalUpdate = 2,     // 可跳过更新
        ForceUpdate = 3,        // 强制更新
    }
}
