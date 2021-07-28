using System;

namespace WebsocketMQTTBridge.JsonInterface.WebClientRequestInterface
{
  [Serializable]
  class WebClientRequest: BaseJsonInterface
  {
    public string command;

    public override string ToString()
    {
      return "command: " + @command;
    }
  }
}
