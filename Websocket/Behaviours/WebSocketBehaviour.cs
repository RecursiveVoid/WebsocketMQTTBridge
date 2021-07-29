using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using WebsocketMQTTBridge.JsonInterface;
using WebsocketMQTTBridge.Util;
using WebsocketMQTTBridge.Mqtt;
using WebsocketMQTTBridge.MqttEventArgs;
using System.Collections.Generic;
using System.Linq;
using WebsocketMQTTBridge.JsonInterface.WebClientRequestInterface;

namespace WebsocketMQTTBridge.Websocket.Behaviours
{
  class WebSocketBehaviour: WebSocketBehavior
  {

    private MqttClient _mqttClient;
    private MqttRequestHandler _mqttRequestHandler;
    private WebsocketResponseCreator _websocketResponseCreator;

    private readonly int SESSION_AMOUNT_LIMIT = 1;

    public WebSocketBehaviour(MqttClient mqttClient)
    {
      _websocketResponseCreator = new WebsocketResponseCreator();
      _mqttClient = mqttClient;
      _mqttClient.onMqttServerResponse += _handleOnMqttServerReponse;
      _mqttRequestHandler = new MqttRequestHandler(_mqttClient);
    }
    protected override void OnClose(CloseEventArgs e)
    {
      base.OnClose(e);
      ConsoleWritter.writeInfo(Sessions.Count.ToString(), "Connection is Closed to Web Client:");
      _mqttClient.onMqttServerResponse -= _handleOnMqttServerReponse;
      _mqttClient.disconnect();
    }

    protected override void OnError(ErrorEventArgs e)
    {
      base.OnError(e);
      var msg = _websocketResponseCreator.createConnectionError();
      Send(msg);
      _mqttClient.onMqttServerResponse -= _handleOnMqttServerReponse;
      _mqttClient.disconnect();
    }
    protected override void OnOpen()
    {
      base.OnOpen();
      var sessionAmount = Sessions.Count;
      var sessionIds = Sessions.IDs.ToList();
      var currentSessionId = sessionIds[sessionAmount - 1];
      if (sessionAmount > SESSION_AMOUNT_LIMIT)
      {

        ConsoleWritter.writeAlert(sessionAmount.ToString(), "Connected Web Client Limit Exceed ");
        ConsoleWritter.writeAlert(sessionAmount.ToString(), "Closing Session... ");
        var msg = _websocketResponseCreator.createConnectionRejectedResponse();
        Send(msg);
        Sessions.CloseSession(currentSessionId);
      } else
      {
        // CHECK IF CONNECTION IS VALID
        var msg = _websocketResponseCreator.createConnectionOkResponse();
        Send(msg);
        ConsoleWritter.writeInfo(sessionAmount.ToString(), "Web Client Connected:");
      }

      
    }

    protected override void OnMessage(MessageEventArgs e)
    {
      var request = e.Data;
      var webRequestExtractor = new WebRequestExtractor();
      var requestType = webRequestExtractor.getRequestType(request);
      var extractedWebRequest = webRequestExtractor.extract(request);
      switch (requestType)
      {
        case WebclientRequestType.MQTT:
          ConsoleWritter.writeRecieved(extractedWebRequest.ToString(), "Request From Websocket Client: ");
          var messageToClient = _websocketResponseCreator.createMqttOkResponse((WebClientRequest) extractedWebRequest);
          Send(messageToClient);
          _mqttRequestHandler.processRequest(extractedWebRequest);
          break;
        case WebclientRequestType.WEBSOCKET:
          break;
        case WebclientRequestType.INVALID:
          var msg = _websocketResponseCreator.createInvalidRequestResponse();
          Send(msg);
          break;
        default:
          break;
      }
    }

    private void _handleOnMqttServerReponse(object sender, MqttServerResponseArgs responseArgs)
    {
      var response = responseArgs.response;
      // TODO convert it to a form of {WTF is going on kinda  +  response }
      Send(response);
    }
  }
}
