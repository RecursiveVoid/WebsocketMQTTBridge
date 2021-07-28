﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebsocketMQTTBridge.Util
{
  static class ConsoleWritter
  {
    // \n
    public static readonly ConsoleColor OK_COLOR = ConsoleColor.Cyan;
    public static readonly ConsoleColor CRITICAL_ERROR_COLOR = ConsoleColor.Red;
    public static readonly ConsoleColor INFO_COLOR = ConsoleColor.Cyan;
    public static readonly ConsoleColor ALERT_COLOR = ConsoleColor.Yellow;
    public static readonly ConsoleColor MESSAGE_COLOR = ConsoleColor.Gray;
    public static readonly ConsoleColor RECIEVED_MESSAGE = ConsoleColor.Magenta;
    public static readonly ConsoleColor SENDED_MESSAGE = ConsoleColor.Green;

    public static void writeOK(string message, string topic = "")
    {
      ConsoleWritter.write(message, OK_COLOR, topic);
    }
    public static void writeCriticalError(string message, string topic = "")
    {
      ConsoleWritter.write(message, CRITICAL_ERROR_COLOR, topic);
    }

    public static void writeInfo(string message, string topic = "")
    {
      ConsoleWritter.write(message, INFO_COLOR, topic);
    }

    public static void writeAlert(string message, string topic = "")
    {
      ConsoleWritter.write(message, ALERT_COLOR, topic);
    }
    public static void writeRecieved(string message, string topic = "")
    {
      ConsoleWritter.write(message, RECIEVED_MESSAGE, topic);
    }

    public static void writeSended(string message, string topic = "")
    {
      ConsoleWritter.write(message, SENDED_MESSAGE, topic);
    }


    private static void newLine()
    {
      Console.Write("\n");
    }

    private static void write(string message, ConsoleColor color, string topic = "")
    {
      ConsoleWritter.newLine();
      if (topic.Length > 0)
      {
        ConsoleWritter.writeTopic(topic, color);
      }
      ConsoleWritter.writeMessage(message);
      ConsoleWritter.newLine();
    }

    private static void writeMessage(string message)
    {
      Console.ForegroundColor = MESSAGE_COLOR;
      Console.Write(message);
    }

    private static void writeTopic(string topic, ConsoleColor color)
    {
      Console.ForegroundColor = color;
      Console.Write(topic);
    }
  }
}
