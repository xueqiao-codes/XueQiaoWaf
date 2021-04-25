using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Events;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace XueQiaoFoundation.UI.Components.ToastNotification
{
    public class NotifierConfigurationWrapper
    {
        public Tuple<INotificationsLifetimeSupervisor> LifetimeSupervisor { get; set; }
        public Tuple<Dispatcher> Dispatcher { get; set; }
        public Tuple<DisplayOptions> DisplayOptions { get; set; }
        public Tuple<IKeyboardEventHandler> KeyboardEventHandler { get; set; }
    }

    public abstract class NotifierWrapper : IDisposable
    {
        protected Action<NotifierConfigurationWrapper> configureAction;

        public NotifierWrapper(Action<NotifierConfigurationWrapper> configureAction)
        {
            this.configureAction = configureAction;
        }

        public void Notify<T>(Func<INotification> createNotificationFunc)
        {
            if (CheckMsgNotifierAvailable())
            {
                GetMsgNotifier()?.Notify<T>(createNotificationFunc);
            }
        }
        
        public virtual void Dispose()
        {

        }

        protected abstract Notifier GetMsgNotifier();

        protected virtual bool CheckMsgNotifierAvailable()
        {
            return true;
        }

        protected static void NotifierConfigurationFillDataFromWrapper(NotifierConfiguration conf, NotifierConfigurationWrapper confWrapper)
        {
            if (confWrapper.LifetimeSupervisor != null)
            {
                conf.LifetimeSupervisor = confWrapper.LifetimeSupervisor.Item1;
            }

            if (confWrapper.Dispatcher != null)
            {
                conf.Dispatcher = confWrapper.Dispatcher.Item1;
            }

            if (confWrapper.DisplayOptions != null)
            {
                conf.DisplayOptions.Width = confWrapper.DisplayOptions.Item1.Width;
                conf.DisplayOptions.TopMost = confWrapper.DisplayOptions.Item1.TopMost;
            }

            if (confWrapper.KeyboardEventHandler != null)
            {
                conf.KeyboardEventHandler = confWrapper.KeyboardEventHandler.Item1;
            }
        }
    }

    /// <summary>
    /// 封装 ControlPositionProvider 类型的 Notifier。以解决 Load 和 Unload 引发的问题
    /// </summary>
    public class NotifierWrapper_ControlPositionProvider : NotifierWrapper
    {
        private Notifier msgNotifier;

        private readonly FrameworkElement trackingElement;
        private readonly Corner corner;
        private readonly double offsetX;
        private readonly double offsetY;

        public NotifierWrapper_ControlPositionProvider(
            FrameworkElement trackingElement,
            Corner corner, double offsetX, double offsetY,
            Action<NotifierConfigurationWrapper> configureAction) : base(configureAction)
        {
            this.trackingElement = trackingElement;
            this.corner = corner;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            trackingElement.Loaded += TrackingElement_Loaded;
            trackingElement.Unloaded += TrackingElement_Unloaded;
            if (trackingElement.IsLoaded)
            {
                SetupMsgNotifier();
            }
        }

        private void TrackingElement_Loaded(object sender, RoutedEventArgs e)
        {
            SetupMsgNotifier();
        }

        private void TrackingElement_Unloaded(object sender, RoutedEventArgs e)
        {
            DisposeMsgNotifier();
        }

        private void SetupMsgNotifier()
        {
            if (msgNotifier == null)
            {
                msgNotifier = new Notifier(conf =>
                {
                    var confWrapper = new NotifierConfigurationWrapper();
                    configureAction?.Invoke(confWrapper);

                    NotifierConfigurationFillDataFromWrapper(conf, confWrapper);

                    var parentWindow = Window.GetWindow(trackingElement) as Window;
                    conf.PositionProvider = new ControlPositionProvider(parentWindow,
                        trackingElement, corner, offsetX, offsetY);
                });
            }
        }

        private void DisposeMsgNotifier()
        {
            if (msgNotifier != null)
            {
                msgNotifier.Dispose();
                msgNotifier = null;
            }
        }

        protected override Notifier GetMsgNotifier()
        {
            return msgNotifier;
        }

        protected override bool CheckMsgNotifierAvailable()
        {
            return msgNotifier != null && trackingElement != null && trackingElement.IsLoaded;
        }

        public override void Dispose()
        {
            base.Dispose();
            DisposeMsgNotifier();
        }
    }


    /// <summary>
    /// 封装 WindowPositionProvider 类型的 Notifier。以解决 Load 和 Unload 引发的问题
    /// </summary>
    public class NotifierWrapper_WindowPositionProvider : NotifierWrapper
    {
        private Notifier msgNotifier;

        private readonly Window window;
        private readonly Corner corner;
        private readonly double offsetX;
        private readonly double offsetY;

        public NotifierWrapper_WindowPositionProvider(
            Window window,
            Corner corner, double offsetX, double offsetY,
            Action<NotifierConfigurationWrapper> configureAction) : base(configureAction)
        {
            this.window = window;
            this.corner = corner;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            window.Loaded += (s, e) => SetupMsgNotifier();
            window.Closed += (s, e) => DisposeMsgNotifier();

            if (window.IsLoaded)
            {
                SetupMsgNotifier();
            }
        }

        private void SetupMsgNotifier()
        {
            if (msgNotifier == null)
            {
                msgNotifier = new Notifier(conf =>
                {
                    var confWrapper = new NotifierConfigurationWrapper();
                    configureAction?.Invoke(confWrapper);

                    NotifierConfigurationFillDataFromWrapper(conf, confWrapper);

                    conf.PositionProvider = new WindowPositionProvider(window, corner, offsetX, offsetY);
                });
            }
        }

        private void DisposeMsgNotifier()
        {
            if (msgNotifier != null)
            {
                msgNotifier.Dispose();
                msgNotifier = null;
            }
        }

        protected override Notifier GetMsgNotifier()
        {
            return msgNotifier;
        }

        protected override bool CheckMsgNotifierAvailable()
        {
            return msgNotifier != null && window != null && window.IsLoaded;
        }

        public override void Dispose()
        {
            base.Dispose();
            DisposeMsgNotifier();
        }
    }
}
