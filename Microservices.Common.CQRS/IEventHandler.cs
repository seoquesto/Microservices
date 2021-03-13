using System.Threading.Tasks;

namespace Microservices.Common.CQRS
{
  public interface IEventHandler<in TEvent> where TEvent : class, IEvent
  {
    Task HandleAsync(TEvent @event);
  }
}