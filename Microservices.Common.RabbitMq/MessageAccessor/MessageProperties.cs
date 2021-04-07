using System.Collections.Generic;

namespace Microservices.Common.RabbitMq.MessageAccessor
{
  public class MessageProperties : IMessageProperties
  {
    public string MessageId { get; set; }
    public string CorrelationId { get; set; }
    public long Timestamp { get; set; }
    public IDictionary<string, object> Headers { get; set; }
  }
}