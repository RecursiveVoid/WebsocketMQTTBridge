using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using WebsocketMQTTBridge.Mqtt;
using WebsocketMQTTBridge.Websocket;

namespace WebsocketMQTTBridge
{
  class App
  {
    private WebsocketServer _webSocketServer;
    public App()
    {
      _init();
    }

    private void _init()
    {
      _prepareTitle(); 
      Console.WriteLine("=============================================================\n");
      _prepareVersionInformation();
      Console.WriteLine("=============================================================\n");
      try
      {
        _prepareWebsocketServer();
        Console.WriteLine(" ");

      }
      catch (Exception e)
      {
        _destroyWebsocketServer();
        Console.ReadKey();
      }
    }

    private void _prepareWebsocketServer()
    {
      _webSocketServer = new WebsocketServer();
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

    private void _prepareVersionInformation()
    {
      var fullNameInfo = getProgramName();
      Console.WriteLine(fullNameInfo);
      Console.WriteLine("Made with love, Original author: M. Ergin Tuerk \n");
    }

    public string getProgramName()
    {
      return Assembly.GetExecutingAssembly().GetName().FullName;
    }

    private void _destroyWebsocketServer()
    {
      _webSocketServer?.destroy();
      _webSocketServer = null;
    }

    public void destroy()
    {
      _destroyWebsocketServer();
    }

  }
}
