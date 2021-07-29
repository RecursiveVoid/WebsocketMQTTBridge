using WebsocketMQTTBridge.JsonInterface.WebClientRequestInterface;

namespace WebsocketMQTTBridge.Websocket
{
  class WebsocketResponseCreator
  {
    public WebsocketResponseCreator()
    {
      // TODO
    }

    public string createConnectionOkResponse()
    {
      return "{\"command\": \"ok\", \"message\": \"socketConnectionOk\"}";
    }

    public string createConnectionRejectedResponse()
    {
      return "{\"command\": \"error\", \"message\": \"socketConnectionRejected.\"}";
    }

    public string createConnectionError()
    {
      return "{\"command\": \"error\", \"message\": \"connectionError.\"}";
    }

    public string createInvalidRequestResponse()
    {
      return "{\"command\": \"error\", \"message\": \"invalidResponse.\"}";
    }

    public string createMqttOkResponse(WebClientRequest request)
    {
      return "{\"command\": \"ok\", \"message\": \"" + request.command + "\"}";
    }
  }
}
