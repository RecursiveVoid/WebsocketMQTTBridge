using System;
using System.Collections.Generic;
using System.Text;

namespace WebsocketMQTTBridge.JsonInterface
{
  [Serializable]
  class WebClientPublishRequest: WebClientRequest
  {
    public string topic;
    public string device;
    public string method;
    public int id;


    public override string ToString()
    {
      return "command: " + @command + ", topic: " + @topic + ", device: " + @device + ", method: " + @method + "id: " + id;
    }
  }
}
