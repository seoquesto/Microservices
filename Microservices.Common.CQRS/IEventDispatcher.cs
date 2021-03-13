using System.Threading.Tasks;

namespace Microservices.Common.CQRS
{
  public interface IEventDispatcher
  {
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
  }
}