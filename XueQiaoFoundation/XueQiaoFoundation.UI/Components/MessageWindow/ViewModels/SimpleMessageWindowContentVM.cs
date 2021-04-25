using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Input;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Components.MessageWindow.Views;

namespace XueQiaoFoundation.UI.Components.MessageWindow.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SimpleMessageWindowContentVM : ViewModel<SimpleMessageWindowContentView>
    {
        [ImportingConstructor]
        public SimpleMessageWindowContentVM(SimpleMessageWindowContentView view) : base(view)
        {
            CaptionHeightHolder = new MessageWindowCaptionHeightHolder();
        }

        public MessageWindowCaptionHeightHolder CaptionHeightHolder { get; private set; }

        private object dialogTitle;
        public object DialogTitle
        {
            get { return dialogTitle; }
            set { SetProperty(ref dialogTitle, value); }
        }
        
        private object message;
        public object Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string buttonTitle;
        public string ButtonTitle
        {
            get { return buttonTitle; }
            set { SetProperty(ref buttonTitle, value); }
        }

        public Action ButtonClickAction { get; set; }
    }
}
