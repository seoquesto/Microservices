using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Common.RabbitMq.Publisher
{
  internal class RabbitMqPublisher : IBusPublisher
  {
    private readonly IBusClient _client;
    private readonly IConventionsProvider _conventionsProvider;

    public RabbitMqPublisher(IBusClient client, IConventionsProvider conventionsProvider)
    {
      _client = client;
      _conventionsProvider = conventionsProvider;
    }

    public Task PublishAsync<T>(T message, string messageId = null, string correlationId = null,
     string spanContext = null, object messageContext = null, IDictionary<string, object> headers = null)
     where T : class
    {
      _client.Send(
        message: message,
        convention: _conventionsProvider.Get(message.GetType()),
        messageId: messageId,
        correlationId: correlationId,
        spanContext: spanContext,
        messageContext: messageContext,
        headers: headers);

      return Task.CompletedTask;
    }
  }
}