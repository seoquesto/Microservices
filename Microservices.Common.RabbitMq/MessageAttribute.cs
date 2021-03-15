using System;

namespace Microservices.Common.RabbitMq
{
  [AttributeUsage(AttributeTargets.Class)]
  public class MessageAttribute : Attribute
  {
    public string Exchange { get; }
    public string RoutingKey { get; }
    public string Queue { get; }

    public MessageAttribute(
      string exchange = null,
      string routingKey = null,
      string queue = null)
    => (this.Exchange, this.RoutingKey, this.Queue) = (exchange, routingKey, queue);
  }
}