using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XueQiaoFoundation.Shared.Model
{
    /// <summary>
    /// Chrome 样式窗体头部数据 holder
    /// </summary>
    public class ChromeWindowCaptionDataHolder : System.Waf.Foundation.Model
    {
        private double captionHeight;
        public double CaptionHeight
        {
            get { return captionHeight; }
            set { SetProperty(ref captionHeight, value); }
        }

        private RoutedEventHandler closeWindowMenuButtonClickHandler;
        public RoutedEventHandler CloseWindowMenuButtonClickHandler
        {
            get { return closeWindowMenuButtonClickHandler; }
            set { SetProperty(ref closeWindowMenuButtonClickHandler, value); }
        }
    }
}
