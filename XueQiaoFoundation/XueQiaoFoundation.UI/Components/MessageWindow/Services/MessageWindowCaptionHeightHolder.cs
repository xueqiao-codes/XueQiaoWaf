using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;

namespace XueQiaoFoundation.UI.Components.MessageWindow.Services
{
    public class MessageWindowCaptionHeightHolder : Model
    {
        private double dialogCaptionHeight;
        public double DialogCaptionHeight
        {
            get { return dialogCaptionHeight; }
            set { SetProperty(ref dialogCaptionHeight, value); }
        }
    }
}
