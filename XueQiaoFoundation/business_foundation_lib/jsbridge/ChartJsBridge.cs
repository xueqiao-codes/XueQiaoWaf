using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace business_foundation_lib.jsbridge
{
    [ComVisible(true)]
    public class ChartJsBridge
    {
        private readonly WebBrowser webBrowser;

        public ChartJsBridge(WebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;
        }

        /// <summary>
        /// 获取 tick 行情
        /// </summary>
        /// <param name="startTimestamp"></param>
        /// <param name="endTimestamp"></param>
        /// <returns></returns>
        public BridgeQuotation[] GetTickQuotations(long reqId, long startTimestamp, long endTimestamp)
        {
            // TODO: 
            throw new NotImplementedException("GetTickQuotations");
        }

        private void Bridge_AddTickQuotations(BridgeQuotation[] quotations)
        {

        }

        private void Bridge_ClearTickQuotations()
        {

        }
        
        private void Bridge_ChartReset()
        {

        }
        
        private void Bridge_ChartDestory()
        {

        }
    }
}
