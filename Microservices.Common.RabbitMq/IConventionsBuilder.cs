using System;

namespace Microservices.Common.RabbitMq
{
  public interface IConventionsBuilder
  {
    string GetRoutingKey(Type type);
    string GetExchange(Type type);
    string GetQueue(Type type);
  }
}