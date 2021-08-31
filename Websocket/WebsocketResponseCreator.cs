using WebsocketMQTTBridge.JsonInterface.WebClientRequestInterface;
using WebsocketMQTTBridge.Mqtt;

namespace WebsocketMQTTBridge.Websocket
{
  class WebsocketResponseCreator
  {
    public WebsocketResponseCreator()
    {
      // TODO
    }

    public string extractWebsocketResponseFromMqttResponse(MqttResponse mqttResponse, WebClientRequest? request)
    {
      switch (mqttResponse)
      {
        case MqttResponse.RESPONSE_OK:
          return "{\"command\": \"ok\", \"from\": \"mqtt\" ,\"message\": \"" + request.command + "\"}";
        case MqttResponse.DISCONNECT:
          return "{\"command\": \"disconnection\",\"message\": \" no active connection to Mqtt broker.\"}";
        case MqttResponse.CONNECTION_REJECTED:
          return "{\"command\": \"error\",\"from\": \"socket\" ,\"message\": \"connectionRejected.\"}";
        case MqttResponse.INVALID_REQUEST:
          return "{\"command\": \"error\", \"from\": \"socket\",\"message\": \"invalidResponse.\"}";
        case MqttResponse.CONNECTION_ERROR:
          return "{\"command\": \"error\", \"from\": \"socket\" ,\"message\": \"connectionError.\"}";
        case MqttResponse.ALREADY_CONNECTED:
          // TODO continue from here
          break;
        case MqttResponse.MQTTCLIENT_UNAVAILABLE:
          break;
        case MqttResponse.ALREADY_UNSUBSCRIBED:
          break;
        case MqttResponse.UNSUBSCRIBED:
          break;
        case MqttResponse.SUBSCRIBED:
          break;
        case MqttResponse.ALREADY_SUBSCRIBED:
          break;
        case MqttResponse.CONNECTION_OK:
          return "{\"command\": \"ok\",\"from\": \"socket\" ,\"message\": \"connectionOk\"}";
          break;
        case MqttResponse.CRITICAL_ERROR:
          break;
        case MqttResponse.NO_SUBSCRIBTIONS:
          break;
        case MqttResponse.MQTTCLIENT_NO_CONNECTION:
          break;
          // TODO
      }
      return "";
    }

    public string createMqttResponse(string response)
    {
      return "{\"command\": \"response\", \"from\": \"mqtt\" ,\"message\":" + response + "}";
    }

    public string createConnectionError() 
    {
      return extractWebsocketResponseFromMqttResponse(MqttResponse.CONNECTION_ERROR, null);
    }

    public string createInvalidRequestResponse()
    {
      return extractWebsocketResponseFromMqttResponse(MqttResponse.INVALID_REQUEST, null);
    }

    public string createConnectionRejectedResponse()
    {
      return extractWebsocketResponseFromMqttResponse(MqttResponse.CONNECTION_REJECTED, null);
    }

    public string createConnectionOkResponse()
    {
      return extractWebsocketResponseFromMqttResponse(MqttResponse.CONNECTION_OK, null);
    }
  }
}

