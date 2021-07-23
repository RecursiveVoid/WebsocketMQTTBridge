using Newtonsoft.Json;
using WebsocketMQTTBridge.JsonInterface;

namespace WebsocketMQTTBridge.Websocket
{
  class WebRequestExtractor
  {
    public WebRequestExtractor()
    {
      // TODO
    }

    public WebClientRequest extract(string request)
    {
      var webClientRequest = JsonConvert.DeserializeObject<WebClientRequest>(request);
      var command = webClientRequest.command.ToLower();
      if (webClientRequest != null)
      {
        switch (command)
        {
          case "connect":
            return JsonConvert.DeserializeObject<WebClientConnectionRequest>(request);
          default:
            // TODO change it to error
            return webClientRequest;
        }
      }
      else
      {
        // TODO change it to error
        return webClientRequest;
      }
    }
  }
}
