using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebsocketMQTTBridge.websocket.behaviours
{
  class DefaultWebSocketBehaviour: WebSocketBehavior
  {

    protected override void OnClose(CloseEventArgs e)
    {
      base.OnClose(e);
    }

    protected override void OnError(ErrorEventArgs e)
    {
      base.OnError(e);
    }
    protected override void OnOpen()
    {
      base.OnOpen();
    }

    protected override void OnMessage(MessageEventArgs e)
    {
      var msg = e.Data == "BALUS"
          ? "I've been balused already..."
          : "I'm not available now.";
      Send(msg);
    }
  }
}
