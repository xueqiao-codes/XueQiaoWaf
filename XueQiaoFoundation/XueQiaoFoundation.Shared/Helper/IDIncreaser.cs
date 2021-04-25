using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public class IDIncreaser
    {
        private readonly long offset;
        private long _id;
        private readonly object _idOperateLock = new object();

        public IDIncreaser(long offset = 0)
        {
            this.offset = offset;
            _id = offset;
        }

        public long Id
        {
            get
            {
                lock (_idOperateLock)
                {
                    return _id;
                }
            }
        }

        public long RequestIncreasedId()
        {
            long x = 0;
            lock (_idOperateLock)
            {
                x = ++_id;
            }
            return x;
        }

        public void Reset()
        {
            lock (_idOperateLock)
            {
                _id = this.offset;
            }
        }
    }
}
