using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoWaf.Trade.Modules.Applications.DataModels
{
    public class IntegerAscendingComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var arg1 = (int)x;
            var arg2 = (int)y;
            if (arg1 < arg2) return -1;
            if (arg1 == arg2) return 0;
            if (arg1 > arg2) return 1;
            return 1;
        }
    }
}
