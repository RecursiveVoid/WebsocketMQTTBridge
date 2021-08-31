using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsocketMQTTBridge.JsonInterface.WebClientRequestInterface
{
  [Serializable]
  class WebClientPublishRequest: WebClientRequest
  {
    public string topic;
    public object message;

    public override string ToString()
    {
      string serilizedMessage = JsonConvert.SerializeObject(message);
      return "\"command\": \"" + @command + "\", \"topic\": \"" + @topic + "\", "+ serilizedMessage + "";
    }
    
  }
}
