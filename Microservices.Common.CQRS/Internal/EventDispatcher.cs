using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.CQRS.Internal
{
  public class EventDispatcher : IEventDispatcher
  {
    private readonly IServiceScopeFactory _factory;
    public EventDispatcher(IServiceScopeFactory factory)
      => this._factory = factory;

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
      using var scope = this._factory.CreateScope();
      var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
      await handler.HandleAsync(@event);
    }
  }
}