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
      return "{\"command\": \"ok\",\"from\": \"socket\" ,\"message\": \"connectionOk\"}";
    }

    public string createConnectionRejectedResponse()
    {
      return "{\"command\": \"error\",\"from\": \"socket\" ,\"message\": \"connectionRejected.\"}";
    }

    public string createConnectionError()
    {
      return "{\"command\": \"error\", \"from\": \"socket\" ,\"message\": \"connectionError.\"}";
    }

    public string createInvalidRequestResponse()
    {
      return "{\"command\": \"error\", \"from\": \"socket\",\"message\": \"invalidResponse.\"}";
    }

    public string createMqttOkResponse(WebClientRequest request)
    {
      return "{\"command\": \"ok\", \"from\": \"mqtt\" ,\"message\": \"" + request.command + "\"}";
    }

    public string createMqttResponse(string response)
    {
      return "{\"command\": \"response\", \"from\": \"mqtt\" ,\"message\":" + response + "}";
    }
  }
}

