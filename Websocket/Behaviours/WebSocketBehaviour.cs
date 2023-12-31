﻿using WebSocketSharp;
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

    private readonly MqttClient _mqttClient;
    private readonly MqttRequestHandler _mqttRequestHandler;
    private readonly WebsocketResponseCreator _websocketResponseCreator;

    private const int SESSION_AMOUNT_LIMIT = 1;

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
      ConsoleWriter.writeInfo(Sessions.Count.ToString(), "Connection is Closed to Web Client:");
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
      // TODO refactor getting currentsession id, looks fishy
      var currentSessionId = sessionIds[sessionAmount - 1];
      if (sessionAmount > SESSION_AMOUNT_LIMIT)
      {
        ConsoleWriter.writeAlert(sessionAmount.ToString(), "Connected Web Client Limit Exceed ");
        ConsoleWriter.writeAlert(sessionAmount.ToString(), "Closing Session... ");
        var msg = _websocketResponseCreator.createConnectionRejectedResponse();
        Send(msg);
        Sessions.CloseSession(currentSessionId);
      } else
      {
        // CHECK IF CONNECTION IS VALID
        var msg = _websocketResponseCreator.createConnectionOkResponseForWebSocket();
        Send(msg);
        ConsoleWriter.writeInfo(sessionAmount.ToString(), "Web Client Connected:");
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
          ConsoleWriter.writeRecieved(extractedWebRequest.ToString(), "Request From Websocket Client: ");
          var mqttResponse = _mqttRequestHandler.processRequest(extractedWebRequest);
          var webClientRequest = (WebClientRequest)extractedWebRequest;
          var responseToClient = _websocketResponseCreator.extractWebsocketResponseFromMqttResponse(mqttResponse, webClientRequest);
          Send(responseToClient);  
          break;
        case WebclientRequestType.WEBSOCKET:
          break;
        case WebclientRequestType.INVALID:
          var msg = _websocketResponseCreator.createInvalidRequestResponse(request);
          Send(msg);
          break;
        default:
          break;
      }
    }

    private void _handleOnMqttServerReponse(object sender, MqttServerResponseArgs responseArgs)
    {
      var response = responseArgs.response;
      var msg = _websocketResponseCreator.createMqttResponse(response);
      Send(msg);
    }
  }
}
