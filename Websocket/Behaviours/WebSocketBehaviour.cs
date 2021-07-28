using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using WebsocketMQTTBridge.JsonInterface;
using WebsocketMQTTBridge.Util;
using WebsocketMQTTBridge.Mqtt;
using WebsocketMQTTBridge.MqttEventArgs;
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
      _mqttClient.onMqttServerResponse += _handleOnMqttServerReponse;
      _mqttRequestHandler = new MqttRequestHandler(_mqttClient);
    }
    protected override void OnClose(CloseEventArgs e)
    {
      base.OnClose(e);
      _mqttClient.onMqttServerResponse -= _handleOnMqttServerReponse;
      _mqttClient.disconnect();
    }

    protected override void OnError(ErrorEventArgs e)
    {
      base.OnError(e);
      _mqttClient.onMqttServerResponse -= _handleOnMqttServerReponse;
      _mqttClient.disconnect();
    }
    protected override void OnOpen()
    {
      base.OnOpen();
    }

    protected override void OnMessage(MessageEventArgs e)
    {
      var request = e.Data;
      var webRequestExtractor = new WebRequestExtractor();
      var extractedWebRequest = webRequestExtractor.extract(request);
      ConsoleWritter.writeRecieved(extractedWebRequest.ToString(), "Request From Websocket Client: ");
      _mqttRequestHandler.processRequest(extractedWebRequest);

      /*
var msg = e.Data == "BALUS"
    ? "I've been balused already..."
    : "I'm not available now.";
Send(msg);
*/
      // TODO extract the command and see what it is
    }

    private void _handleOnMqttServerReponse(object sender, MqttServerResponseArgs responseArgs)
    {
      var response = responseArgs.response;
      // TODO convert it to a form of {WTF is going on kinda  +  response }
      Send(response);
    }
  }
}
