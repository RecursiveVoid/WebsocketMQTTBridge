using System;

namespace WebsocketMQTTBridge.JsonInterface
{
  [Serializable]
  class WebClientRequest
  {
    public string command;

    public override string ToString()
    {
      return "command: " + @command;
    }
  }
}
