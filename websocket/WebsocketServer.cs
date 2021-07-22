using System;
using WebSocketSharp;
using WebSocketSharp.Server;

using WebsocketMQTTBridge.util;

namespace WebsocketMQTTBridge
{
  // TODO create sepearete classes
  public class ExampleWebSocketBehaviour : WebSocketBehavior
  {
    protected override void OnMessage(MessageEventArgs e)
    {
      var msg = e.Data == "BALUS"
                ? "I've been balused already..."
                : "I'm not available now.";
      Send(msg);
    }
  }
  /* https://github.com/PingmanTools/websocket-sharp/*/
  class WebsocketServer
  {

    private string _ipAdress;
    private int _port;
    private WebSocketServer _websocketServer;

    public WebsocketServer(string ipAdress = "127.0.0.1", int port = 80)
    {
      _ipAdress = ipAdress;
      _port = port;
      _init();
    }

    private void _init()
    {
      try
      {
        ConsoleWritter.writeInfo(_ipAdress + ":" + _port.ToString(), "Creating Websocket server on: ");
        _websocketServer = new WebSocketServer("ws://" + _ipAdress + ":" + _port.ToString());
        ConsoleWritter.writeOK("OK", "Websocket Server Running: ");
        _websocketServer.Start();
        _websocketServer.AddWebSocketService<ExampleWebSocketBehaviour>("/");
      }
      catch (Exception e)
      {
        ConsoleWritter.writeCriticalError(e.ToString(), "Websocket Server ERROR: ");
      }
    }

    public void stop()
    {
      ConsoleWritter.writeAlert(" ", "Stopping Websocket Server");
      if (_websocketServer == null) return;
      _websocketServer.Stop();
    }

    public void destroy()
    {
      ConsoleWritter.writeAlert(" ", "Destroying Websocket Server");
      stop();
      _websocketServer = null;
    }

  }
}
