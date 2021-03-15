namespace Microservices.Common.RabbitMq
{
  public interface IBusClient
  {
    void Send(object message, IConvention convention, string messageId = null, string correlationId = null);
  }
}