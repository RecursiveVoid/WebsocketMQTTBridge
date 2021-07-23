using System;
using System.Collections.Generic;
using System.Text;

namespace WebsocketMQTTBridge.JsonInterface
{
  [Serializable]
  class WebClientConnectionRequest: WebClientRequest
  {
    string ip;
    int port;
    string clientId;
  }
}
