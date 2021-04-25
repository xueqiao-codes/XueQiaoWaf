using ContainerShell.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.mailbox.user.message.thriftapi;
using XueQiaoFoundation.Shared.Helper;

namespace ContainerShell.Applications.Services
{
    [Export, Export(typeof(IUserMessageService)), PartCreationPolicy(CreationPolicy.Shared)]
    public class UserMessageService : Model, IUserMessageService
    {
        public UserMessageService()
        {
            MessageItems = new ObservableCollection<UserMessage>();
            CollectionChangedEventManager.AddHandler(MessageItems, MessageCollectionChanged);
            InvalidateUnreadMessageItemCount();
        }

        public ObservableCollection<UserMessage> MessageItems { get; private set; }

        private int unreadMessageItemCount;
        public int UnreadMessageItemCount
        {
            get { return unreadMessageItemCount; }
            set { SetProperty(ref unreadMessageItemCount, value); }
        }

        private void MessageCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.OfType<UserMessage>().ToArray();
            var newItems = e.NewItems?.OfType<UserMessage>().ToArray();

            if (oldItems?.Any() == true)
            {
                foreach (var i in oldItems)
                {
                    PropertyChangedEventManager.RemoveHandler(i, MessageItemPropChanged, "");
                }
            }

            if (newItems?.Any() == true)
            {
                foreach (var i in newItems)
                {
                    PropertyChangedEventManager.RemoveHandler(i, MessageItemPropChanged, "");
                    PropertyChangedEventManager.AddHandler(i, MessageItemPropChanged, "");
                }
            }

            InvalidateUnreadMessageItemCount();
        }

        private void MessageItemPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserMessage.State))
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => 
                {
                    InvalidateUnreadMessageItemCount();
                });
            }
        }

        private void InvalidateUnreadMessageItemCount()
        {
            this.UnreadMessageItemCount = MessageItems.Count(i => i.State != MessageState.READ);
        }
    }
}
