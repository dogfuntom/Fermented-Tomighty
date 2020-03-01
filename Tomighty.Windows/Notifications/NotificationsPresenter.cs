//
//  Tomighty - http://www.tomighty.org
//
//  This software is licensed under the Apache License Version 2.0:
//  http://www.apache.org/licenses/LICENSE-2.0.txt
//

using DesktopNotifications;
using Tomighty.Events;
using Tomighty.Windows.Events;
using Tomighty.Windows.Timer;
using Windows.UI.Notifications;

namespace Tomighty.Windows.Notifications
{
    internal class NotificationsPresenter
    {
        private readonly IPomodoroEngine pomodoroEngine;
        private readonly IUserPreferences userPreferences;
        private readonly ToastNotifier toastNotifier = DesktopNotificationManagerCompat.CreateToastNotifier();

        public NotificationsPresenter(IPomodoroEngine pomodoroEngine, IUserPreferences userPreferences, IEventHub eventHub)
        {
            this.pomodoroEngine = pomodoroEngine;
            this.userPreferences = userPreferences;
            eventHub.Subscribe<TimerStopped>(OnTimerStopped);
            eventHub.Subscribe<AppUpdated>(OnAppUpdated);
            eventHub.Subscribe<FirstRun>(OnFirstRun);
        }

        private void OnFirstRun(FirstRun @event)
        {
            var toast = Toasts.FirstRun();
            toastNotifier.Show(toast);
        }

        private void OnAppUpdated(AppUpdated @event)
        {
            toastNotifier.Show(Toasts.AppUpdated());
        }

        private void OnTimerStopped(TimerStopped @event)
        {
            if (@event.IsIntervalCompleted && userPreferences.ShowToastNotifications)
            {
                var toast = Toasts.IntervalCompleted(@event.IntervalType, pomodoroEngine.SuggestedBreakType);

                TimerWindow.DispatcherUi.BeginInvoke(new System.Action
                        (() => toastNotifier.Show(toast))
                   );
            }
        }
    }
}
