using System;
using System.Collections.Generic;
using System.Text;

namespace WebsocketMQTTBridge.Mqtt
{
  public enum MqttResponse
  {
    CONNECTION_ERROR,
    DISCONNECT,
    RESPONSE_OK,
    INVALID_RESPONSE,
    CONNECTION_REJECTED,
    ALREADY_CONNECTED,
    MQTTCLIENT_UNAVAILABLE,
    ALREADY_UNSUBSCRIBED,
    UNSUBSCRIBED,
    SUBSCRIBED,
    ALREADY_SUBSCRIBED,
    CONNECTION_OK,
    CRITICAL_ERROR,
    NO_SUBSCRIBTIONS,
  }
}
