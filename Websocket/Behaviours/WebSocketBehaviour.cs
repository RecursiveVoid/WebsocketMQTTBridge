using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using WebsocketMQTTBridge.JsonInterface;
using WebsocketMQTTBridge.Util;
using WebsocketMQTTBridge.Mqtt;
using System.Collections.Generic;

namespace WebsocketMQTTBridge.Websocket.Behaviours
{
  class WebSocketBehaviour: WebSocketBehavior
  {

    private MqttClient _mqttClient;
    private MqttRequestHandler _mqttRequestHandler;

    public WebSocketBehaviour(MqttClient mqttClient)
    {
      _mqttClient = mqttClient;
      _mqttRequestHandler = new MqttRequestHandler(_mqttClient);
    //  _prepareMqttClient();
    }
    protected override void OnClose(CloseEventArgs e)
    {
      base.OnClose(e);
      _mqttClient.disconnect();
      // _mqttClient.disconnect();
    }

    protected override void OnError(ErrorEventArgs e)
    {
      base.OnError(e);
      _mqttClient.disconnect();
    }
    protected override void OnOpen()
    {
      base.OnOpen();
      _prepareMqttClient();
    }

    protected override void OnMessage(MessageEventArgs e)
    {
      var request = e.Data;
      var webRequestExtractor = new WebRequestExtractor();
      var extractedWebRequest = webRequestExtractor.extract(request);
      ConsoleWritter.writeInfo(extractedWebRequest.ToString(), "RECIEVED FROM CLIENT:");
      _mqttRequestHandler.processRequest(extractedWebRequest);

      /*
var msg = e.Data == "BALUS"
    ? "I've been balused already..."
    : "I'm not available now.";
Send(msg);
*/
      // TODO extract the command and see what it is
    }

    private void _prepareMqttClient()
    {
      if (!_mqttClient.isConnected())
      {
        _mqttClient.connect();
        _subscribeToMqttBroker();
      }
    }

    private void _subscribeToMqttBroker()
    {
      List<string> topics = new List<string> { "events/inputs", "res/version", "res/peripherals/iapi", "peripherals/iapi" };
      _mqttClient.subscribeTopics(topics);
    }
  }
}
