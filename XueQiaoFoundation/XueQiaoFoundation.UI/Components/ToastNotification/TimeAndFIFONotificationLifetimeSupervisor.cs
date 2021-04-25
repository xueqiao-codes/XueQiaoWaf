using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Lifetime.Clear;
using ToastNotifications.Utilities;

namespace XueQiaoFoundation.UI.Components.ToastNotification
{
    /// <summary>
    /// 提醒 LifetimeSupervisor。提醒只显示限定时间，并且只显示限定数量提醒。当提醒数大于限定数时，按照先进先出原则剔除旧的提醒。
    /// </summary>
    public class TimeAndFIFONotificationLifetimeSupervisor : INotificationsLifetimeSupervisor
    {
        private readonly TimeSpan _notificationLifetime;
        private readonly int _maximumNotificationCount;

        private Dispatcher _dispatcher;
        private NotificationsList _notifications;

        private IInterval _interval;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="notificationLifetime"></param>
        /// <param name="maximumNotificationCount"></param>
        public TimeAndFIFONotificationLifetimeSupervisor(TimeSpan notificationLifetime, int maximumNotificationCount)
        {
            _notifications = new NotificationsList();

            _notificationLifetime = notificationLifetime;
            _maximumNotificationCount = maximumNotificationCount;

            _interval = new Interval();
        }

        public void PushNotification(INotification notification)
        {
            if (_disposed)
            {
                Debug.WriteLine($"Warn ToastNotifications {this}.{nameof(PushNotification)} is already disposed");
                return;
            }

            if (_interval.IsRunning == false)
                TimerStart();
            
            int numberOfNotificationsToClose = Math.Max(_notifications.Count - _maximumNotificationCount + 1, 0);

            var notificationsToRemove = _notifications
                .OrderBy(x => x.Key)
                .Take(numberOfNotificationsToClose)
                .Select(x => x.Value)
                .ToList();

            foreach (var n in notificationsToRemove)
                CloseNotification(n.Notification);

            _notifications.Add(notification);
            RequestShowNotification(new ShowNotificationEventArgs(notification));
        }

        public void CloseNotification(INotification notification)
        {
            _notifications.TryRemove(notification.Id, out var removedNotification);
            RequestCloseNotification(new CloseNotificationEventArgs(removedNotification.Notification));
        }


        private bool _disposed = false;
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _interval?.Stop();
            _interval = null;
            _notifications?.Clear();
            _notifications = null;
        }



        public void UseDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        protected virtual void RequestShowNotification(ShowNotificationEventArgs e)
        {
            ShowNotificationRequested?.Invoke(this, e);
        }

        protected virtual void RequestCloseNotification(CloseNotificationEventArgs e)
        {
            CloseNotificationRequested?.Invoke(this, e);
        }

        private void TimerStart()
        {
            _interval.Invoke(TimeSpan.FromMilliseconds(200), OnTimerTick, _dispatcher);
        }

        private void TimerStop()
        {
            _interval.Stop();
        }

        private void OnTimerTick()
        {
            TimeSpan now = DateTimeNow.Local.TimeOfDay;

            var notificationsToRemove = _notifications
                .Where(x => x.Value.Notification.CanClose && x.Value.CreateTime + _notificationLifetime <= now)
                .Select(x => x.Value)
                .ToList();

            foreach (var n in notificationsToRemove)
                CloseNotification(n.Notification);

            if (_notifications.IsEmpty)
                TimerStop();
        }

        public void ClearMessages(IClearStrategy clearStrategy)
        {
            var notifications = clearStrategy.GetNotificationsToRemove(_notifications);
            foreach (var notification in notifications)
            {
                CloseNotification(notification);
            }
        }

        public event EventHandler<ShowNotificationEventArgs> ShowNotificationRequested;
        public event EventHandler<CloseNotificationEventArgs> CloseNotificationRequested;
    }
}
