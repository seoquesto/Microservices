using System.Threading.Tasks;

namespace Microservices.Common.RabbitMq
{
  public interface IBusPublisher
  {
    Task PublishAsync<T>(T message, string messageId = null, string correlationId = null) where T : class;
  }
}