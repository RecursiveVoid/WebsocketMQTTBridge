using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebsocketMQTTBridge.JsonInterface;
using WebsocketMQTTBridge.JsonInterface.WebClientRequestInterface;
using WebsocketMQTTBridge.Util;

namespace WebsocketMQTTBridge.Mqtt
{
  public class MqttRequestHandler
  {
    private MqttClient _mqttClient;

    public MqttRequestHandler(MqttClient mqttClient)
    {
      _mqttClient = mqttClient;
    }

    public MqttResponse processRequest(BaseJsonInterface baseRequest)
    {
      if (_mqttClient == null) 
      {
        return MqttResponse.MQTTCLIENT_UNAVAILABLE;
      }
      if (baseRequest is WebClientConnectionRequest webClientConnectionRequest)
      {
        return _handleConnectionRequest(webClientConnectionRequest);
      } else if (baseRequest is WebClientSubscriptionRequest webClientSubscriptionRequest)
      {
        return _handleSubscriptionRequest(webClientSubscriptionRequest);
      } else if(baseRequest is WebClientPublishRequest webClientPublish)
      {
        return _handlePublishRequest(webClientPublish);
      } else if(baseRequest is WebClientRequest webClientRequest)
      {
        if (webClientRequest.command == "disconnect")
        {
          return _handleDisconnectRequest();
        }
      }
      return MqttResponse.INVALID_REQUEST;
    }

    private MqttResponse _handlePublishRequest(WebClientPublishRequest webClientPublishRequest)
    {
      if (_mqttClient.isConnected())
      {
        string topic = webClientPublishRequest.topic;
        string message = JsonConvert.SerializeObject(webClientPublishRequest.message);
        ConsoleWriter.writeSended("topic: "+ topic +", message: " + message, "Request To MQTT Server: ");
        return _mqttClient.publish(topic, message);
      }
      else
      {
        ConsoleWriter.writeAlert("Client is disconnected.", "MQTT Client Cannot Subscribe");
        return MqttResponse.MQTTCLIENT_NO_CONNECTION;
      }
    }

    private MqttResponse _handleDisconnectRequest()
    {
      if (_mqttClient.isConnected())
      {
        ConsoleWriter.writeSended("","Disconnection request processing...");
        return _mqttClient.disconnect();
      }
      return MqttResponse.MQTTCLIENT_NO_CONNECTION;
    }

    private MqttResponse _handleSubscriptionRequest(WebClientSubscriptionRequest webClientSubscriptionRequest)
    {
      try
      {
        if (_mqttClient.isConnected())
        {
          var loweredCommand = webClientSubscriptionRequest.command.ToLower();
          if (loweredCommand == "subscribe")
          {
            var topics = new List<string>(webClientSubscriptionRequest.topics);
            return _mqttClient.subscribeTopics(topics);
          }
          else
          {
            var topics = new List<string>(webClientSubscriptionRequest.topics);
            return _mqttClient.unSubscribeTopics(topics);
          }
        }
        else
        {
          ConsoleWriter.writeAlert(" Client is disconnected.", "MQTT Client Cannot Subscribe");
          return MqttResponse.MQTTCLIENT_NO_CONNECTION;
        }
      }
      catch (Exception e)
      {
        ConsoleWriter.writeCriticalError(e.ToString(), "MQTT Client Subscription ERROR: ");
        return MqttResponse.CRITICAL_ERROR;
      }
    }

    private MqttResponse _handleConnectionRequest(WebClientConnectionRequest webClientConnectionRequest)
    {
      if (!_mqttClient.isConnected())
      {
        return _mqttClient.connect(webClientConnectionRequest.ip, webClientConnectionRequest.port, webClientConnectionRequest.clientId);
      }
      else
      {
        ConsoleWriter.writeInfo(" Please disconnect and re-connect.", "MQTT Client is aready connected.");
        return MqttResponse.ALREADY_CONNECTED;
      }
    }
  }
}
