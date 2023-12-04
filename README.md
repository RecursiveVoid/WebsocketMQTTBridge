# WebsocketMQTTBridge
[![License][license-src]][license-href]
[![Bundle size][bundlephobia-src]][bundlephobia-href]
![screenshot of runtime console](https://github.com/RecursiveVoid/gifs/blob/main/websocketMQTTBridge/terminal_screenshot.png?raw=true)

## About
WebsocketMQTTBridge is state of art, straight forward tool to create connection and relay a MQTT broker through Websocket.  

## What's in the box
MQTT broker and WS server support.

## What's missing
Supporting MQTTS brokers and WSS server. 

## Default server parameters
Default websocket server is created on: ```ws://127.0.0.1:80```  
IP: ``` 127.0.0.1 ```
PORT:  ```80```

## Commands
Commands are in Json format.
In given examples, the parameters are for demonstrating the data type only.
### Connection request to MQTT broker
```
{
  "command": "connect",
  "ip": "127.0.0.1",
  "port": 10,
  "clientId": "1"
}
```
### Subscription/Unsubscribtion Request 
The command subscribe works in same structure with unsubscribe.
```
{
  "command": "subscribe", 
  "topics:" ["topic1", "topic2"]
}
```
```
{
  "command": "unsubscribe", 
  "topics:" ["topic1", "topic2"]
}
```
### Publishing message to given topic
```
{
  "command": "publish",
  "topic": "topic1",
  "message": { message is an object, preferably a JSON }
}
```
[license-src]: https://badgen.net/github/license/amio/badgen
[license-href]: LICENSE.md
[bundlephobia-src]: https://badgen.net/bundlephobia/minzip/badgen
[bundlephobia-href]: https://bundlephobia.com/result?p=badgen