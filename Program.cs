using System;
using System.Collections.Generic;
using WebsocketMQTTBridge.Util;

namespace WebsocketMQTTBridge
{
  class Program
  {
    
    static void Main(string[] args)
    {
      var app = new App();
      var output = Console.ReadLine();
      if (output.ToLower().Equals("exit")) {
        app.destroy();
        ConsoleWritter.writeInfo("Press any key to exit", "The System has been stopped... ");
      }
      Console.ReadLine();
      // System.Threading.Thread.Sleep(10000);
    }
  }
}
