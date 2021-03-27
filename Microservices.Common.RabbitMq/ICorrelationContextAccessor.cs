namespace Microservices.Common.RabbitMq
{
  public interface ICorrelationContextAccessor
  {
    object CorrelationContext { get; set; }
  }
}