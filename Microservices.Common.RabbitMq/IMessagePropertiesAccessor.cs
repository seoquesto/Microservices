namespace Microservices.Common.RabbitMq
{
  public interface IMessagePropertiesAccessor
  {
    IMessageProperties MessageProperties { get; set; }
  }
}