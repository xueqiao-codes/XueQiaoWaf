using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace NativeModel.Trade
{
    public class NativeComposeViewDetail : Model
    {
        private NativeComposeGraph composeGraph;
        public NativeComposeGraph ComposeGraph
        {
            get { return composeGraph; }
            set { SetProperty(ref composeGraph, value); }
        }

        private NativeComposeView userComposeView;
        public NativeComposeView UserComposeView
        {
            get { return userComposeView; }
            set { SetProperty(ref userComposeView, value); }
        }
    }
}
