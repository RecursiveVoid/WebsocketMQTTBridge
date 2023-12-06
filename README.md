# WebsocketMQTTBridge
[![License][license-src]][license-href]
[![Release][release-src]][release-href]
[![Dependencies][Dependencies-src]][Dependencies-href]
![screenshot of runtime console](https://github.com/RecursiveVoid/gifs/blob/main/websocketMQTTBridge/terminal_screenshot.png?raw=true)

## About
WebsocketMQTTBridge is state of art, straightforward tool to create connection and relay MQTT broker communication through Websocket.

## Why Websocket - MQTT Bridge?
One day at work a regular everyday normal project, created a regular everday normal problem which needed a regular everyday normal solution.
And the solution was the _blazing fast_ _WebsocketMQTTBridge_.

## What's in the box
MQTT broker and WS server support.

## What's missing & TODOs
* Supporting MQTTS brokers and WSS server.
* Configurating the websocket server through json and console application. 

## Default server parameters
Default websocket server is created on: ```ws://127.0.0.1:80```  
IP: ``` 127.0.0.1 ```
PORT:  ```80```
Server parameters can be changed in source code, under _WebsocketServer.cs_
## Commands
Commands are in Json format.
In given examples, the parameters are for demonstrating the data type only.
### Connection request to MQTT broker
```json
{ 
  "command": "connect",
  "ip": "127.0.0.1",
  "port": 10,
  "clientId": "1"
}
```
### Subscription/Unsubscribtion Request 
The command subscribe works in same structure with unsubscribe.
```json
{
  "command": "subscribe", 
  "topics:" ["topic1", "topic2"]
}
```
```json
{
  "command": "unsubscribe", 
  "topics:" ["topic1", "topic2"]
}
```
### Publishing message to given topic
```json
{
  "command": "publish",
  "topic": "topic1",
  "message": { "customParameter": "message is a json object" }
}
```
## Dependencies used for the project

* Used for serializing/deserializing json requests/responses to objects. [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
* Creating websocket server. [websocket-sharp](https://github.com/sta/websocket-sharp)
* Creating communication to MQTT broker. [paho.mqtt.m2mqtt](https://github.com/eclipse/paho.mqtt.m2mqtt)

[license-src]: https://badgen.net/static/license/MIT/blue
[license-href]: LICENSE
[release-src]: https://badgen.net/static/Release/0.1.0/orange
[release-href]: WebsocketMQTTBridge.csproj
[Dependencies-src]: https://badgen.net/static/Dependencies/3/blue
[Dependencies-href]:WebsocketMQTTBridge.csproj
