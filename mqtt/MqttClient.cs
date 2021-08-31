using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;
using WebsocketMQTTBridge.MqttEventArgs;
using WebsocketMQTTBridge.Util;

namespace WebsocketMQTTBridge.Mqtt
{
  public class MqttClient
  {

    private uPLibrary.Networking.M2Mqtt.MqttClient _mqttClient;
    private List<string> _subscribedTopics;

    public event EventHandler<MqttServerResponseArgs> onMqttServerResponse;

    public MqttClient()
    {
      _subscribedTopics = new List<string>();
    }

    public MqttResponse connect(string brokerIp = "127.0.0.1", int brokerPort = 1883, string clientName = "client")
    {
      if (isConnected())
      {
        return MqttResponse.ALREADY_CONNECTED;
      }
      try
      {
        ConsoleWriter.writeInfo(brokerIp + ":" + brokerPort.ToString(), "Connecting Mqtt Server on: ");
        _mqttClient = new uPLibrary.Networking.M2Mqtt.MqttClient(brokerIp, brokerPort, false, null, null, uPLibrary.Networking.M2Mqtt.MqttSslProtocols.None);
        _mqttClient.Connect(clientName);
        ConsoleWriter.writeInfo("MQTT Connection Closed", "Listening Event: ");
        _mqttClient.ConnectionClosed += _handleConnectionClosed;
        ConsoleWriter.writeInfo("MQTT Subscribed", "Listening Event: ");
        _mqttClient.MqttMsgSubscribed += _handleSubscribed;
        ConsoleWriter.writeInfo("MQTT Publish Recieved", "Listening Event: ");
        _mqttClient.MqttMsgPublishReceived += _handlePublishRecieved;
        if (_mqttClient.IsConnected)
        {
          ConsoleWriter.writeOK(_mqttClient.ClientId, "MQTT Client Running: ");
          return MqttResponse.CONNECTION_OK;
        }
      } catch (Exception e)
      {
        ConsoleWriter.writeCriticalError(e.ToString(), "MQTT Client ERROR: ");
        return MqttResponse.CRITICAL_ERROR;
      }
      return MqttResponse.CONNECTION_ERROR;
    }

    private void _handleConnectionClosed(object sender, EventArgs e)

    {
      ConsoleWriter.writeAlert(" ", "MQTT Client Disconnected");
    }


    public MqttResponse disconnect() // there might be a error
    {
      if (_mqttClient == null) return MqttResponse.MQTTCLIENT_UNAVAILABLE;
      ConsoleWriter.writeAlert(" ", "Stopping MQTT Client");
      try
      {
        unsubscribe();
        _disconnectMqttClient();
        _destroyEvents();
        return MqttResponse.DISCONNECT;
      }
      catch (Exception e)
      {
        ConsoleWriter.writeCriticalError(e.ToString(), "MQTT Client ERROR: ");
        return MqttResponse.CRITICAL_ERROR;
      }

    }

    private void _disconnectMqttClient()
    {
      if (isConnected())
      {
        _mqttClient.Disconnect();
      }
    }

    private void _destroyEvents()
    {
      if (_mqttClient == null) return;
      ConsoleWriter.writeAlert("MQTT Connection Closed", "Unlistening Event: ");
      _mqttClient.ConnectionClosed -= _handleConnectionClosed;
      ConsoleWriter.writeAlert("MQTT Subscribed", "Unlistening Event: ");
      _mqttClient.MqttMsgSubscribed -= _handleSubscribed;
      ConsoleWriter.writeAlert("MQTT Publish Recieved", "Unlistening Event: ");
      _mqttClient.MqttMsgPublishReceived -= _handlePublishRecieved;
    }

    public MqttResponse subscribeTopics(List<string> topics)
    {
      if (_mqttClient == null) 
      { 
        ConsoleWriter.writeCriticalError("Server Connection not avaliable", "Cannot Subscribe to MQTT topic: ");
        return MqttResponse.MQTTCLIENT_UNAVAILABLE;
      }
      try
      {

        var qosLevel = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;
        ConsoleWriter.writeInfo(qosLevel.ToString(), "Qos Level: ");
        byte[] qosLevels = new byte[topics.Count];
        for (int i = topics.Count - 1; i >= 0; i--)
        {
          if (_subscribedTopics.Contains(topics[i]))
          {
            ConsoleWriter.writeAlert(topics[i], "MQTT already subscribe to topic: ");
            topics.RemoveAt(i);
          }
          else
          {
            qosLevels[i] = qosLevel;
            ConsoleWriter.writeInfo(topics[i], "MQTT Subscribing to: ");
            _subscribedTopics.Add(topics[i]);
          }
        }
        if (topics.Count <= 0) return MqttResponse.ALREADY_SUBSCRIBED;
        _mqttClient.Subscribe(topics.ToArray(), qosLevels);
        return MqttResponse.SUBSCRIBED;
      }
      catch (Exception e)
      {
        ConsoleWriter.writeCriticalError(e.ToString(), "MQTT Client Subscription ERROR: ");
        return MqttResponse.CRITICAL_ERROR;
      }
    }

    public MqttResponse unsubscribe()
    {
      if (_mqttClient == null) 
      { 
        ConsoleWriter.writeCriticalError("Server Connection not avaliable", "Cannot unSubscribe to MQTT topic: ");
        return MqttResponse.MQTTCLIENT_UNAVAILABLE;
      }
      if (_subscribedTopics == null || _subscribedTopics.Count <= 0) 
      {
        ConsoleWriter.writeCriticalError("No Subscribtion", "Cannot unSubscribe to MQTT topic: "); 
        return MqttResponse.NO_SUBSCRIBTIONS; 
      }
      try
      {
        ConsoleWriter.writeAlert(_subscribedTopics.ToString(), "Unsubscribing from all subscribed topics from MQTT: ");
        _mqttClient.Unsubscribe(_subscribedTopics.ToArray());
        _subscribedTopics.Clear();
        return MqttResponse.UNSUBSCRIBED;
      }
      catch (Exception e)
      {
        ConsoleWriter.writeCriticalError(e.ToString(), "MQTT Client Un-Subscription ERROR: ");
        return MqttResponse.CRITICAL_ERROR;
      }
    }

    private void _handleSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
      ConsoleWriter.writeInfo(e.MessageId.ToString(), "Subscribed for ID: ");
    }

    private void _handlePublishRecieved(object sender, MqttMsgPublishEventArgs e)
    {
      // convert string to objects like JSON and forward to websocket..]\
      try
      {
        var message = Encoding.UTF8.GetString(e.Message);
        ConsoleWriter.writeRecieved(message, "Response from Mqtt Server: ");
        var mqttServerResnponseArgs = new MqttServerResponseArgs(message);
        onMqttServerResponse?.Invoke(this, mqttServerResnponseArgs);
      }
      catch (Exception exepction)
      {
        ConsoleWriter.writeCriticalError(exepction.ToString(), "MQTT Client publish-recived ERROR: ");
      }
    }

    public MqttResponse publish(string topic, string msg)
    {
      try
      {
        msg = msg.Replace("'", "\"");
        _mqttClient.Publish(topic, Encoding.UTF8.GetBytes(msg));
        return MqttResponse.RESPONSE_OK;
      }
      catch (Exception e)
      {
        ConsoleWriter.writeCriticalError(e.ToString(), "MQTT Client Publishing ERROR: ");
        return MqttResponse.INVALID_REQUEST;
      }
    }

    public bool isConnected()
    {
      return _mqttClient == null ? false : _mqttClient.IsConnected;
    }

    public void destroy()
    {
      ConsoleWriter.writeAlert(" ", "Destroying MQTT Client");
      disconnect();
      _mqttClient = null;
    }

  }
}
