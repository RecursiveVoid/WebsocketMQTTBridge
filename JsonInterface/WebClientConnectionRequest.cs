using System;

namespace WebsocketMQTTBridge.JsonInterface
{
  [Serializable]
  class WebClientConnectionRequest: WebClientRequest
  {
    public string ip;
    public int port;
    public string clientId;

    public override string ToString()
    {
      return "command: " + @command  + ", ip: " + @ip + ", port: " + port + ", clientId: " + @clientId;
    }
  }
}
