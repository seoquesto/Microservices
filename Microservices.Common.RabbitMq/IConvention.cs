using System;

namespace Microservices.Common.RabbitMq
{
  public interface IConvention
  {
    Type Type { get; }
    string RoutingKey { get; }
    string Exchange { get; }
    string Queue { get; }
  }
}