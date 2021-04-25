using ContainerShell.Applications.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using xueqiao.mailbox.user.message.thriftapi;

namespace ContainerShell.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserMessageContentVM : ViewModel<IUserMessageContentView>
    {
        [ImportingConstructor]
        public UserMessageContentVM(IUserMessageContentView view) : base(view)
        {
        }
        
        private UserMessage messageItem;
        public UserMessage MessageItem
        {
            get { return messageItem; }
            set { SetProperty(ref messageItem, value); }
        }

        public void UpdateContentWithHtml(string htmlContent) => ViewCore.UpdateContentWithHtml(htmlContent);

        private bool isContentDownloading;
        public bool IsContentDownloading
        {
            get { return isContentDownloading; }
            set { SetProperty(ref isContentDownloading, value); }
        }
    }
}
