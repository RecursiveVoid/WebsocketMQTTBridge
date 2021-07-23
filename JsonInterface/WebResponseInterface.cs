using System;
using System.Collections.Generic;
using System.Text;

namespace WebsocketMQTTBridge.JsonInterface
{
  [Serializable]
  class JsonWebSubscribe
  {
    public string command;
    public string[] topic;
  }

  [Serializable]
  class JsonWebMessageRecieve
  {
    public string command;
    public string topic;
    public string message;
  }

  [Serializable]
  class JsonWebPublish
  {
    // TODO think about the commands
  }
}
