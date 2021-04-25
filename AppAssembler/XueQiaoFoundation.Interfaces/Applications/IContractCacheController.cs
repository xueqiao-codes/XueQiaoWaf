using NativeModel.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Interface;

namespace XueQiaoFoundation.Interfaces.Applications
{
    public interface IContractCacheController : ICacheController<int, NativeContract>
    {
    }
}
