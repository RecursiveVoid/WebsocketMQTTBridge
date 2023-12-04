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

    public string extractWebsocketResponseFromMqttResponse(MqttResponse mqttResponse, WebClientRequest request)
    {
      switch (mqttResponse)
      {
        case MqttResponse.RESPONSE_OK:
          return "{\"command\": \"ok\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.DISCONNECT:
          return "{\"command\": \"disconnect\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.CONNECTION_REJECTED:
          return "{\"command\": \"connectionRejected\",\"from\": \"socket\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.INVALID_REQUEST:
          return "{\"command\": \"invalidRequest\", \"from\": \"socket\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.CONNECTION_ERROR:
          return "{\"command\": \"connectionError\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.ALREADY_CONNECTED:
          return "{\"command\": \"alreadyConnected\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.MQTTCLIENT_UNAVAILABLE:
          return "{\"command\": \"mqttClientUnavailable\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.ALREADY_UNSUBSCRIBED:
          // get the webclientRequest (cast it) unsubscribe shit, and send back what you unsubscribed
          return "{\"command\": \"alreadyUnsubscribed\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.UNSUBSCRIBED:
          // get the webclientRequest (cast it) unsubscribe shit, and send back what you unsubscribed
          return "{\"command\": \"unsubscribed\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.SUBSCRIBED:
          // same
          return "{\"command\": \"subscribed\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.ALREADY_SUBSCRIBED:
          return "{\"command\": \"alreadySubscribed\", \"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.CONNECTION_OK:
          return "{\"command\": \"connectionOK\",\"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.CRITICAL_ERROR:
          return "{\"command\": \"criticalError\",\"from\": \"socket\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.NO_SUBSCRIBTIONS:
          return "{\"command\": \"noSubscribtions\",\"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        case MqttResponse.MQTTCLIENT_NO_CONNECTION:
          return "{\"command\": \"mqttClientNoConnection\",\"from\": \"mqtt\" ,\"for\": \"" + request.command + "\"}";
        default:
          return "{\"command\": \"unknownState\",\"from\": \"[socket, mqtt]\" ,\"for\": \"" + request.command + "\"}";
      }
    }

    public string createMqttResponse(string response)
    {
      return "{\"command\": \"invalidRequest\", \"from\": \"mqtt\" ,\"message\":" + response + "}";
    }

    public string createConnectionError() 
    {
      return extractWebsocketResponseFromMqttResponse(MqttResponse.CONNECTION_ERROR, null);
    }

    public string createInvalidRequestResponse(string request)
    {
      return "{\"command\": \"error\", \"from\": \"socket\",\"message\": \"invalidResponse.\",  ,\"for\": \"" + request + "\"}";
    }

    public string createConnectionRejectedResponse()
    {
      return extractWebsocketResponseFromMqttResponse(MqttResponse.CONNECTION_REJECTED, null);
    }

    public string createConnectionOkResponseForWebSocket()
    {
      return "{\"command\": \"connectionOK\",\"from\": \"websocket\"}";
    }
  }
}

