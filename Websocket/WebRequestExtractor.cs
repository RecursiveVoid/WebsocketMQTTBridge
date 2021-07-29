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

    public WebclientRequestType getRequestType(string request)
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
                return WebclientRequestType.MQTT;
              case "subscribe":
                return WebclientRequestType.MQTT;
              case "publish":
                return WebclientRequestType.MQTT;
              default:
                // TODO change it to error
                return WebclientRequestType.INVALID;
            }
          }
          else
          {
            // TODO change it to error
            return WebclientRequestType.INVALID;
          }
        }
        else
        {
          // TODO change it to error
          return WebclientRequestType.INVALID;
        }

      }
      catch (Exception e)
      {
        return WebclientRequestType.INVALID;
      }
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
