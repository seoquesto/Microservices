using System;

namespace Microservices.Common.RabbitMq
{
  public class MessageConvention : IConvention
  {
    public Type Type { get; set; }
    public string RoutingKey { get; set; }
    public string Exchange { get; set; }
    public string Queue { get; set; }

    public MessageConvention(Type type, string routingKey, string exchange, string queue)
    {
      Type = type;
      RoutingKey = routingKey;
      Exchange = exchange;
      Queue = queue;
    }
  }
}