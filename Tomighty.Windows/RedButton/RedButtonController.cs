using System;
using System.Text;
using Tomighty.Events;
using Tomighty.Windows.Events;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Tomighty.Windows.RedButton {
    public class RedButtonController {
        readonly SynchronousEventHub eventHub;
        MQTTClient mqtt;

        public RedButtonController(SynchronousEventHub eventHub) {
            this.eventHub = eventHub;
            eventHub.Subscribe<TimeElapsed>(OnTimeElasped);

        }

        void OnTimeElasped(TimeElapsed obj) {
            
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
            mqtt.Subscribe("esp");
        }

        void onMsgReceived(object sender, MqttMsgPublishEventArgs e) {
            string message = Encoding.UTF8.GetString(e.Message);
            if (message == "toast")
                eventHub.Publish(new RedButtonConnectionChanged(true));
        }

        public void Disconnect() {
            if (mqtt != null && mqtt.client.IsConnected) {
                mqtt.Disconnect();
            }
        }
    }
}
