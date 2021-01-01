using System.Runtime.InteropServices;
using DesktopNotifications;

// The GUID CLSID must be unique to your app. Create a new GUID if copying this code.
namespace Tomighty.Windows.Notifications
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("f1c09466-e472-45be-8248-c88309824a46"), ComVisible(true)]
    public class MyNotificationActivator : NotificationActivator
    {
        public override void OnActivated(string invokedArgs, NotificationUserInput userInput, string appUserModelId)
        {
            // dummy activator
            if (Toasts.TimerAction.WithArgs.ContainsKey(invokedArgs))
            {
                var timerAction = Toasts.TimerAction.WithArgs[invokedArgs];

                PomodoroEngine.pomodoroEngine.StartTimer(timerAction.IntervalType);
            }
        }
    }
}
