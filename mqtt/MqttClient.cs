using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;
using WebsocketMQTTBridge.Util;

namespace WebsocketMQTTBridge.Mqtt
{
  class MqttClient
  {

    private uPLibrary.Networking.M2Mqtt.MqttClient _mqttClient;
    private List<string> _subscribedTopics;


    public MqttClient()
    {
      _subscribedTopics = new List<string>();
    }

    public void connect(string brokerIp = "127.0.0.1", int brokerPort = 1883, string clientName = "client")
    {
      if (isConnected()) return;
      try
      {
        ConsoleWritter.writeInfo(brokerIp + ":" + brokerPort.ToString(), "Connecting Mqtt Server on: ");
        _mqttClient = new uPLibrary.Networking.M2Mqtt.MqttClient(brokerIp, brokerPort, false, null, null, uPLibrary.Networking.M2Mqtt.MqttSslProtocols.None);
        _mqttClient.Connect(clientName);
        _mqttClient.ConnectionClosed += _handleConnectionClosed;
        _mqttClient.MqttMsgSubscribed += _handleSubscribed;
        _mqttClient.MqttMsgPublishReceived += _handlePublishRecieved;
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


    public void disconnect() // there might be a error
    {
      if (_mqttClient == null) return;
      ConsoleWritter.writeAlert(" ", "Stopping MQTT Client");
      try
      {
        unsubscribe();
        _mqttClient.Disconnect();
        _mqttClient.ConnectionClosed -= _handleConnectionClosed;
        _mqttClient.MqttMsgSubscribed -= _handleSubscribed;
        _mqttClient.MqttMsgPublishReceived -= _handlePublishRecieved;
      }
      catch (Exception e)
      {
        ConsoleWritter.writeCriticalError(e.ToString(), "MQTT Client ERROR: ");
      }

    }

    public void subscribeTopics(List<string> topics)
    {
      if (_mqttClient == null) { ConsoleWritter.writeCriticalError("Server Connection not avaliable", "Cannot Subscribe to MQTT topic: "); return; }
      var qosLevel = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;
      ConsoleWritter.writeInfo(qosLevel.ToString(), "Qos Level: ");
      byte[] qosLevels = new byte[topics.Count];
      for (int i = topics.Count - 1; i >= 0; i--)
      {
        if (_subscribedTopics.Contains(topics[i]))
        {
          topics.RemoveAt(i);
        } else
        {
          qosLevels[i] = qosLevel;
          ConsoleWritter.writeInfo(topics[i], "Subscribing to: ");
          _subscribedTopics.Add(topics[i]);
        }
      }
      if (topics.Count <= 0) return;
      _mqttClient.Subscribe(topics.ToArray(), qosLevels);
    }

    public void unsubscribe()
    {
      if (_mqttClient == null) { ConsoleWritter.writeCriticalError("Server Connection not avaliable", "Cannot unSubscribe to MQTT topic: "); return; }
      if (_subscribedTopics == null || _subscribedTopics.Count <= 0) { ConsoleWritter.writeCriticalError("No Subscribtion", "Cannot unSubscribe to MQTT topic: "); return; }
      ConsoleWritter.writeInfo(_subscribedTopics.ToString(), "Unsubscribing from all subscribed topics from MQTT: ");
      _mqttClient.Unsubscribe(_subscribedTopics.ToArray());
    }

    private void _handleSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
      ConsoleWritter.writeInfo(e.MessageId.ToString(), "Subscribed for ID: ");
    }

    private void _handlePublishRecieved(object sender, MqttMsgPublishEventArgs e)
    {
      // convert string to objects like JSON
      ConsoleWritter.writeInfo(Encoding.UTF8.GetString(e.Message), "MQTT SERVER RESPONSE: ");
    }

    public void publish(string topic, string msg)
    {
      msg = msg.Replace("'", "\"");
      _mqttClient.Publish(topic, Encoding.UTF8.GetBytes(msg));
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
