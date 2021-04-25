using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShell.Applications.DataModels
{
    /// <summary>
    /// 选择文件的声音类型
    /// </summary>
    public enum SelectFileSoundType
    {
        OrderTraded = 1,
        OrderTriggered = 2,
        OrderErr = 3,
        LameTraded = 4,
        OrderAmbiguous = 5,
        OrderOtherNotify = 100,
    }
}
