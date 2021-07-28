using Newtonsoft.Json;
using System;
using WebsocketMQTTBridge.JsonInterface;
using WebsocketMQTTBridge.JsonInterface.WebClientRequestInterface;

namespace WebsocketMQTTBridge.Websocket
{
  class WebRequestExtractor
  {
    public WebRequestExtractor()
    {
      // TODO
    }

    public BaseJsonInterface extract(string request)
    {
      try 
      {
        BaseJsonInterface baseRequest = JsonConvert.DeserializeObject<BaseJsonInterface>(request);
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
              case "subscribe":
                return JsonConvert.DeserializeObject<WebClientSubscriptionRequest>(request);
              case "publish":
                return JsonConvert.DeserializeObject<WebClientPublishRequest>(request);
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
        return new BaseJsonInterface();
      }
    }
  }
}
