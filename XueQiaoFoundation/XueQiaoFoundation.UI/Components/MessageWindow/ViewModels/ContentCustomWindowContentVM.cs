using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using XueQiaoFoundation.UI.Components.MessageWindow.Services;
using XueQiaoFoundation.UI.Components.MessageWindow.Views;

namespace XueQiaoFoundation.UI.Components.MessageWindow.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    internal class ContentCustomWindowContentVM : ViewModel<ContentCustomWindowContentView>
    {
        [ImportingConstructor]
        public ContentCustomWindowContentVM(ContentCustomWindowContentView view) : base(view)
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

        private object contentCustomView;
        public object ContentCustomView
        {
            get { return contentCustomView; }
            set { SetProperty(ref contentCustomView, value); }
        }

        private bool isHideCloseDialogMenuButton;
        public bool IsHideCloseDialogMenuButton
        {
            get { return isHideCloseDialogMenuButton; }
            set { SetProperty(ref isHideCloseDialogMenuButton, value); }
        }

    }
}
