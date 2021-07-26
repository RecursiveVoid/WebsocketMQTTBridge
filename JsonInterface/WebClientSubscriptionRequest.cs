using System;
using System.Collections.Generic;
using System.Text;

namespace WebsocketMQTTBridge.JsonInterface
{
  [Serializable]
  class WebClientSubscriptionRequest: WebClientRequest
  {
    public string[] topics;

    public override string ToString()
    {

      string requestTopics = "";
      foreach(string topic in topics)
      {
        requestTopics += " ," + topic;
      }
      return "command: " + @command + "Topics:[ " + @requestTopics + " ]";
    }
  }
}