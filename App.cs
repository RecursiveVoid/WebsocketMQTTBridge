using System;
using System.Collections.Generic;
using System.Text;

namespace WebsocketMQTTBridge
{
  class App
  {
    private WebsocketServer _webSocketServer;
    private MqttClient _mqttClient;
    public App()
    {
      _init();
    }

    private void _init()
    {
      _prepareTitle();
      try
      {
        _webSocketServer = new WebsocketServer();
        Console.WriteLine(" ");
        _mqttClient = new MqttClient();
        if(_mqttClient.isConnected())
        {
          _subscribe();
        }
      }
      catch (Exception e)
      {
        _webSocketServer?.destroy();
        _mqttClient?.destroy();
        _webSocketServer = null;
        _mqttClient = null;
        Console.ReadKey();
      }
    }

    private void _subscribe()
    {
      List<string> topics = new List<string> { "events/inputs", "res/version", "res/peripherals/iapi", "peripherals/iapi" };
      _mqttClient.subscribeTopics(topics);
    }

    private void _prepareTitle()
    {
      string title = @"                    __  __                   __                    __        __     __         _     __
   ____ ___  ____ _/ /_/ /_   _      _____  / /_  _________  _____/ /_____  / /_   / /_  _____(_)___/ /___ ____
  / __ `__ \/ __ `/ __/ __/  | | /| / / _ \/ __ \/ ___/ __ \/ ___/ //_/ _ \/ __/  / __ \/ ___/ / __  / __ `/ _ \
 / / / / / / /_/ / /_/ /_    | |/ |/ /  __/ /_/ (__  ) /_/ / /__/ ,< /  __/ /_   / /_/ / /  / / /_/ / /_/ /  __/
/_/ /_/ /_/\__, /\__/\__/    |__/|__/\___/_.___/____/\____/\___/_/|_|\___/\__/  /_.___/_/  /_/\__,_/\__, /\___/
             /_/                                                                                   /____/";
      Console.WriteLine(title);
    }

  }
}
