using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace business_foundation_lib.quotationpush
{
    // 自增ID生成器
    public class IDMaker
    {
        static long safeInstanceCount = 0;

        static public long nextID()
        {
            Interlocked.Increment(ref safeInstanceCount);
            return safeInstanceCount;
        }
    }
}
