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

    public void processRequest(BaseJsonInterface baseRequest)
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
        if (webClientPublishRequest.parameters != null)
        {
          var parametersToString = string.Join(",", webClientPublishRequest.parameters);
          string normalizeParameters = string.Join(",", parametersToString.Split(',').Select(x => string.Format("'{0}'", x)).ToList());
          string request = "{ 'device': '" + webClientPublishRequest.device + "', 'method': '" + webClientPublishRequest.method + "', 'id': " + webClientPublishRequest.id + ", 'params': ["+ normalizeParameters + "] }";
          ConsoleWriter.writeSended(request, "Request To MQTT Server: ");
          _mqttClient.publish(webClientPublishRequest.topic, request);
        } else
        {
          string request = "{ 'device': '" + webClientPublishRequest.device + "', 'method': '" + webClientPublishRequest.method + "', 'id': " + webClientPublishRequest.id + " }";

          ConsoleWriter.writeSended(request, "Request To MQTT Server: ");
          _mqttClient.publish(webClientPublishRequest.topic, request);
        }
      }
      else
      {
        ConsoleWriter.writeAlert("Client is disconnected.", "MQTT Client Cannot Subscribe");
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
        ConsoleWriter.writeAlert(" Client is disconnected.", "MQTT Client Cannot Subscribe");
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
        ConsoleWriter.writeInfo(" Please disconnect and re-connect.", "MQTT Client is aready connected.");
      }
    }
  }
}
