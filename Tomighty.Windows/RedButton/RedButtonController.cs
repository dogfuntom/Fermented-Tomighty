using System;
using System.Text;
using System.Threading.Tasks;
using Tomighty.Events;
using Tomighty.Windows.Events;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Tomighty.Windows.RedButton {
    public class RedButtonController {
        readonly ITimer timer;
        readonly SynchronousEventHub eventHub;
        MQTTClient mqtt;
        readonly IPomodoroEngine pomodoroEngine;
        private const string mqttTopic = "esp";

        public RedButtonController(ITimer timer, IPomodoroEngine pomodoroEngine, SynchronousEventHub eventHub) {
            this.timer = timer;
            this.pomodoroEngine = pomodoroEngine;
            this.eventHub = eventHub;
            eventHub.Subscribe<TimerStarted>(OnTimerStarted);
            eventHub.Subscribe<TimerStopped>(OnTimerStopped);

        }

        public void Connect() {
            mqtt = new MQTTClient("m10.cloudmqtt.com", 13633);
            mqtt.Connect("***REMOVED***", "***REMOVED***");
            if (!mqtt.client.IsConnected) {
                eventHub.Publish(new RedButtonConnectionChanged(false));
                return;
            }
            eventHub.Publish(new RedButtonConnectionChanged(true));
            
            mqtt.client.MqttMsgPublishReceived += onMsgReceived;
            mqtt.Subscribe(mqttTopic);
            mqtt.Send(mqttTopic, "tomighty online");
        }

        void onMsgReceived(object sender, MqttMsgPublishEventArgs e) {
            string message = Encoding.UTF8.GetString(e.Message);
            switch (message) {
                case "btn press":
                    if (timer.RemainingTime == Duration.Zero)
                        Task.Run(() => pomodoroEngine.StartTimer(IntervalType.Pomodoro));
                    else
                        Task.Run(() => pomodoroEngine.StartTimer(IntervalType.ShortBreak));
                    break;
            }
        }

        void OnTimerStarted(TimerStarted @event) {
            if (@event.IntervalType == IntervalType.ShortBreak || @event.IntervalType == IntervalType.LongBreak)
                mqtt.Send(mqttTopic, "led blink");
            else
                mqtt.Send(mqttTopic, "led on");
        }

        void OnTimerStopped(TimerStopped @event) {
            mqtt.Send(mqttTopic, "led off");
        }


        public void Disconnect() {
            if (mqtt != null && mqtt.client.IsConnected) {
                mqtt.Disconnect();
            }
        }
    }
}
