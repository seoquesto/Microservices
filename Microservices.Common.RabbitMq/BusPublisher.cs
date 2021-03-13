using System.Threading.Tasks;

namespace Microservices.Common.RabbitMq
{
  internal sealed class RabbitMqPublisher : IBusPublisher
  {
    private readonly IBusClient _client;
    private readonly IConventionsProvider _conventionsProvider;

    public RabbitMqPublisher(IBusClient client, IConventionsProvider conventionsProvider)
    {
      _client = client;
      _conventionsProvider = conventionsProvider;
    }

    public Task PublishAsync<T>(T message, string messageId = null, string correlationId = null) where T : class
    {
      _client.Send(message, _conventionsProvider.Get(message.GetType()));

      return Task.CompletedTask;
    }
  }
}