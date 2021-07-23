using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using WebsocketMQTTBridge.JsonInterface;
using WebsocketMQTTBridge.Util;
using WebsocketMQTTBridge.Mqtt;

namespace WebsocketMQTTBridge.Websocket.Behaviours
{
  class WebSocketBehaviour: WebSocketBehavior
  {

    public WebSocketBehaviour()
    {

    }
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
      var webRequestExtractor = new WebRequestExtractor();
      var request = e.Data;
      var extractedRequest = webRequestExtractor.extract(request);
      ConsoleWritter.writeInfo(extractedRequest.ToString(), "RECIEVED FROM CLIENT:");
      /*
var msg = e.Data == "BALUS"
    ? "I've been balused already..."
    : "I'm not available now.";
Send(msg);
*/
      // TODO extract the command and see what it is
    }
  }
}
