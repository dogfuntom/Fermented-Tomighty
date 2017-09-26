print('--- main.py started ---')

from machine import Pin, idle, reset, Timer
from network import WLAN

# https://raw.githubusercontent.com/micropython/micropython-lib/mqtt/umqtt.simple/umqtt/simple.py
from mqtt import MQTTClient
import socket
import time


WIFI_INFO = '***REMOVED***', '***REMOVED***'

MQTT_BROKER_INFO = dict(client_id='ESP8266-%d' % time.time(),
                        server='m10.cloudmqtt.com',
                        port=13633,
                        user='***REMOVED***',
                        password='***REMOVED***')
MQTT_TOPIC = b'esp'

# ---------------------------------------------------------------------------- #

STA = WLAN(0)  # {0: STA, 1: AP}
STA.active(True)
STA.connect(*WIFI_INFO)

print('Connecting to %s Wi-Fi...' % WIFI_INFO[0])
while not STA.isconnected():
    idle()
print('Connected! My IP:', STA.ifconfig()[0])

led = Pin(12, Pin.OUT)
button = Pin(16, Pin.IN)
button_state_prev = button.value()
button_state_now = button_state_prev

# ---------------------------------------------------------------------------- #

blink_timer = Timer(-1)


def on_message(topic, msg):
    print('%s -> %s' % (str(topic), msg))
    if topic == MQTT_TOPIC:
        if msg == b'led blink':
            print('Starting LED blink')
            blink_timer.init(period=2000, mode=Timer.PERIODIC, callback=lambda _:led.value(not led.value()))
        else:
            blink_timer.deinit()

        if msg == b'led on':
            print('Turning LED ON')
            led.value(0)
        elif msg == b'led off':
            print('Turning LED OFF')
            led.value(1)
        elif msg == b'tomighty online':
            print('Tomighty is online!')
            short_blink()


def short_blink():
    for i in range(4):
        led.off()  # actually on
        time.sleep_ms(100)
        led.on()  # actually off 
        time.sleep_ms(100)

mq = MQTTClient(**MQTT_BROKER_INFO)
mq.set_callback(on_message)
mq.connect()

mq.subscribe(MQTT_TOPIC, qos=1)
mq.publish(MQTT_TOPIC, 'device online', qos=1)

print('Waiting for messagges in "%s" topic' % MQTT_TOPIC)

short_blink()


while True:
    mq.check_msg()

    button_state_prev = button_state_now
    button_state_now = button.value()
    if button_state_prev == 1 and button_state_now == 0:
        print('Button press detected')
        mq.publish(MQTT_TOPIC, 'btn press', qos=1)
