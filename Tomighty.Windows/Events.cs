namespace Tomighty.Windows.Events
{
    public class FirstRun { }
    public class AppUpdated { }
    public class RedButtonConnectionChanged {
        public bool Connected;
        public RedButtonConnectionChanged(bool connected) {
            Connected = connected;
        }
    }
}
