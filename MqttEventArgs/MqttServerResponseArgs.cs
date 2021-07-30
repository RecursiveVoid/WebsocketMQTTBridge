using System;
using System.Collections.Generic;
using System.Text;

namespace WebsocketMQTTBridge.MqttEventArgs
{
  public class MqttServerResponseArgs: EventArgs
  {
    public string response { get; private set; }
    public MqttServerResponseArgs(string serverResponse)
    {
      response = serverResponse;
    }
  }
}
