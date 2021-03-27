using System.Collections.Generic;

namespace Microservices.Common.RabbitMq
{
  public interface IBusClient
  {
    void Send(object message, IConvention convention, string messageId = null, string correlationId = null,
         string spanContext = null, object messageContext = null, IDictionary<string, object> headers = null);
  }
}