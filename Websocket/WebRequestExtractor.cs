using Newtonsoft.Json;
using System;
using WebsocketMQTTBridge.JsonInterface;

namespace WebsocketMQTTBridge.Websocket
{
  class WebRequestExtractor
  {
    public WebRequestExtractor()
    {
      // TODO
    }

    public BaseRequest extract(string request)
    {
      try 
      {
        BaseRequest baseRequest = JsonConvert.DeserializeObject<BaseRequest>(request);
        if (baseRequest != null)
        {
          WebClientRequest webClientRequest = JsonConvert.DeserializeObject<WebClientRequest>(request);
          if (webClientRequest != null)
          {
            var command = webClientRequest.command.ToLower();
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
        else
        {
          // TODO change it to error
          return baseRequest;
        }

      } catch(Exception e)
      {
        return new BaseRequest();
      }
    }
  }
}
