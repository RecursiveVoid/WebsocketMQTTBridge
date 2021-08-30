using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsocketMQTTBridge.JsonInterface.WebClientRequestInterface
{
  [Serializable]
  class WebClientPublishRequest: WebClientRequest
  {
    public string topic;
    public string device;
    public string method;
    public int id;
    public string[] parameters;


    public override string ToString()
    {
      if (parameters != null)
      {
        var parametersToString = string.Join(",", parameters);var normalizeParamters = parametersToString.Replace(",", "','") + "'";
        string normalizeParameters = string.Join(",", parametersToString.Split(',').Select(x => string.Format("'{0}'", x)).ToList());
        return "\"command\": \"" + @command + "\", \"topic\": \"" + @topic + "\", \"device\": \"" + @device + "\", \"method\": \"" + @method + "\", \"id\":\" " + id + "\", \"params\": [" + normalizeParameters + "]";
      }
      return "\"command\": \"" + @command + "\", \"topic\": \"" + @topic + "\", \"device\": \"" + @device + "\", \"method\": \"" + @method + "\", \"id\":\" " + id+ "\"";
    }
  }
}
