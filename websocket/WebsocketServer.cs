using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using WebsocketMQTTBridge.Util;
using WebsocketMQTTBridge.Websocket.Behaviours;
using WebsocketMQTTBridge.Mqtt;


namespace WebsocketMQTTBridge.Websocket
{
 
  /* https://github.com/PingmanTools/websocket-sharp/*/
  class WebsocketServer
  {

    private string _ipAdress;
    private int _port;
    private WebSocketServer _websocketServer;
    private MqttClient _mqttClient;

    public WebsocketServer()
    {
      _ipAdress = "127.0.0.1"; // create the server on localhost 
      _port = 80; // default port is 80
      _init();
    }

    public WebsocketServer(int port = 80)
    {
      _ipAdress = "127.0.0.1"; // create the server on localhost 
      _port = port;
      _init();
    }

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
        ConsoleWriter.writeInfo(_ipAdress + ":" + _port.ToString(), "Creating Websocket server on: ");
        _websocketServer = new WebSocketServer("ws://" + _ipAdress + ":" + _port.ToString());
        ConsoleWriter.writeOK("OK", "Websocket Server Running: ");
        _websocketServer.Start();
        _mqttClient = new MqttClient();
        _websocketServer.AddWebSocketService("/", () => new WebSocketBehaviour(_mqttClient));
      }
      catch (Exception e)
      {
        ConsoleWriter.writeCriticalError(e.ToString(), "Websocket Server ERROR: ");
      }
    }

    public void stop()
    {
      ConsoleWriter.writeAlert(" ", "Stopping Websocket Server");
      if (_websocketServer == null) return;
      _websocketServer.Stop();
      _mqttClient.disconnect();

    }

    private void _destroyMqttClient()
    {
      _mqttClient?.destroy();
      _mqttClient = null;
    }

    public void destroy()
    {
      ConsoleWriter.writeAlert(" ", "Destroying Websocket Server");
      stop();
      _destroyMqttClient();
      _websocketServer = null;
    }

  }
}
