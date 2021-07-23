﻿using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;
using WebsocketMQTTBridge.Util;

namespace WebsocketMQTTBridge.Mqtt
{
  class MqttClient
  {
    private string _clientName;

    private uPLibrary.Networking.M2Mqtt.MqttClient _mqttClient;


    public MqttClient()
    {
      // TODO
    }

    public void connect(string brokerIp = "127.0.0.1", int brokerPort = 1883)
    {
      if (isConnected()) return;
      try
      {
        ConsoleWritter.writeInfo(brokerIp + ":" + brokerPort.ToString(), "Connecting Mqtt Server on: ");
        _clientName = "MQTTClient";
        _mqttClient = new uPLibrary.Networking.M2Mqtt.MqttClient(brokerIp, brokerPort, false, null, null, uPLibrary.Networking.M2Mqtt.MqttSslProtocols.None);
        _mqttClient.Connect(_clientName);
        _mqttClient.ConnectionClosed += _handleConnectionClosed;
        if (_mqttClient.IsConnected)
        {
          ConsoleWritter.writeOK(_mqttClient.ClientId, "MQTT Client Running: ");
        }
  
      } catch (Exception e)
      {
        ConsoleWritter.writeCriticalError(e.ToString(), "MQTT Client ERROR: ");
      }
    }

    private void _handleConnectionClosed(object sender, EventArgs e)

    {
      ConsoleWritter.writeAlert(" ", "MQTT Client Disconnected");
    }


    public void disconnect()
    {
      if (_mqttClient == null) return;
      ConsoleWritter.writeAlert(" ", "Stopping MQTT Client");
      try
      {
        if (_mqttClient.IsConnected)
        _mqttClient?.Disconnect();
      }
      catch (Exception e)
      {
        ConsoleWritter.writeCriticalError(e.ToString(), "MQTT Client ERROR: ");
      }

    }

    public void subscribeTopics(List<string> topics)
    {
      if (_mqttClient == null) { ConsoleWritter.writeAlert("Server Connection not avaliable", "Cannot Subscribe to MQTT topic: "); return; }
      var qosLevel = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;
      ConsoleWritter.writeInfo(qosLevel.ToString(), "Qos Level: ");
      byte[] qosLevels = new byte[topics.Count];
      topics.ForEach(topic =>
      {
        ConsoleWritter.writeInfo(topic, "Subscribing to: ");
        var topicIndex = topics.IndexOf(topic);
        qosLevels[topicIndex] = qosLevel;
      });
      _mqttClient.Subscribe(topics.ToArray(), qosLevels);
      _mqttClient.MqttMsgSubscribed += _handleSubscribed;
      _mqttClient.MqttMsgPublishReceived += _handlePublishRecieved;
    }

    private void _handleSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
      ConsoleWritter.writeInfo(e.MessageId.ToString(), "Subscribed for ID: ");
    }

    private void _handlePublishRecieved(object sender, MqttMsgPublishEventArgs e)
    {
      // TODO
    }

    public bool isConnected()
    {
      return _mqttClient == null ? false : _mqttClient.IsConnected;
    }

    public void destroy()
    {
      ConsoleWritter.writeAlert(" ", "Destroying MQTT Client");
      disconnect();
      _mqttClient = null;
    }

  }
}
