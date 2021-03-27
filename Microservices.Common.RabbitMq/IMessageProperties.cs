using System.Collections.Generic;

namespace Microservices.Common.RabbitMq
{
  public class IMessageProperties
  {
    string MessageId { get; set; }
    string CorrelationId { get; set; }
    long Timestamp { get; set; }
    IDictionary<string, object> Headers { get; set; }
  }
}