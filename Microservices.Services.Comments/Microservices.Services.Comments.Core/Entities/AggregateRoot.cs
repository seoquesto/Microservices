using System.Collections.Generic;
using System.Linq;
using Microservices.Services.Comments.Core.Events;

namespace Microservices.Services.Comments.Core.Entities
{
  public abstract class AggregateRoot
  {
    private List<IDomainEvent> _events = new List<IDomainEvent>();
    public IEnumerable<IDomainEvent> Events => _events;
    public AggregateId Id { get; protected set; }
    public int Version { get; protected set; }

    protected void AddEvent(IDomainEvent @event)
    {
      if (!_events.Any())
      {
        Version++;
      }

      _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();
  }
}