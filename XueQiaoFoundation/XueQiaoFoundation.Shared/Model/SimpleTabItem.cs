using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Model
{
    public class SimpleTabItem : System.Waf.Foundation.Model
    {
        private object header;
        public object Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        private object contentView;
        public object ContentView
        {
            get { return contentView; }
            set { SetProperty(ref contentView, value); }
        }
    }
}
