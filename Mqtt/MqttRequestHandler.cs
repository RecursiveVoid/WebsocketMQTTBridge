using System;
using System.Collections.Generic;
using System.Text;
using WebsocketMQTTBridge.JsonInterface;
using WebsocketMQTTBridge.Util;

namespace WebsocketMQTTBridge.Mqtt
{
  class MqttRequestHandler
  {
    private MqttClient _mqttClient;
    public MqttRequestHandler(MqttClient mqttClient)
    {
      _mqttClient = mqttClient;
    }

    public void processRequest(BaseRequest baseRequest)
    {
      if (_mqttClient == null) return;
      if (baseRequest is WebClientConnectionRequest webClientConnectionRequest)
      {
        _handleConnectionRequest(webClientConnectionRequest);
      } else if (baseRequest is WebClientSubscriptionRequest webClientSubscriptionRequest)
      {
        _handleSubscriptionRequest(webClientSubscriptionRequest);
      } else if(baseRequest is WebClientPublishRequest webClientPublish)
      {
        _handlePublishRequest(webClientPublish);
      }
    }

    private void _handlePublishRequest(WebClientPublishRequest webClientPublishRequest)
    {
      if (_mqttClient.isConnected())
      {
        // TODO convert it to a method or something
        string request = "{ 'device': '"+ webClientPublishRequest .device+"', 'method': '"+ webClientPublishRequest .method+"', 'id': "+ webClientPublishRequest .id+" }";
        ConsoleWritter.writeSended(request, "Request To MQTT Server: ");
        _mqttClient.publish(webClientPublishRequest.topic, request);
      }
      else
      {
        ConsoleWritter.writeAlert("Client is disconnected.", "MQTT Client Cannot Subscribe");
      }
    }

    private void _handleSubscriptionRequest(WebClientSubscriptionRequest webClientSubscriptionRequest)
    {
      if (_mqttClient.isConnected())
      {
        var topics = new List<string>(webClientSubscriptionRequest.topics);
        _mqttClient.subscribeTopics(topics);
      }
      else
      {
        ConsoleWritter.writeAlert(" Client is disconnected.", "MQTT Client Cannot Subscribe");
      }
    }

    private void _handleConnectionRequest(WebClientConnectionRequest webClientConnectionRequest)
    {
      if (!_mqttClient.isConnected())
      {
        _mqttClient.connect(webClientConnectionRequest.ip, webClientConnectionRequest.port, webClientConnectionRequest.clientId);
      }
      else
      {
        ConsoleWritter.writeInfo(" Please disconnect and re-connect.", "MQTT Client is aready connected.");
      }
    }
  }
}
