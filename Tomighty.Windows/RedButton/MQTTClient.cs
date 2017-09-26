using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Tomighty.Windows.RedButton {
    class MQTTClient {
        public MqttClient client;

        public MQTTClient(string host, int port) {
            client = new MqttClient(host, port, false, null, null, MqttSslProtocols.None);
        }

        public byte Connect(string login, string password) => client.Connect(Guid.NewGuid().ToString(), login, password);
        public void Disconnect() => client.Disconnect();

        public ushort Send(string topic, string message) => client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
        public ushort Subscribe(string topic) => client.Subscribe(new string[] { topic },
                                                                  new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }
}
