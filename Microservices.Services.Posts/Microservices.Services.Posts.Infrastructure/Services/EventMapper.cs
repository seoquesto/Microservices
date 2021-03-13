using System.Collections.Generic;
using System.Linq;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Application.Services;
using Microservices.Services.Posts.Core.Events;

using ApplicationEvents = Microservices.Services.Posts.Application.Events;

namespace Microservices.Services.Posts.Infrastructure.Services
{
  public class EventMapper : IEventMapper
  {
    public IEvent Map(IDomainEvent @event)
      => @event switch {
        PostCreated e => new ApplicationEvents.PostCreated { Id = e.Post.Id },
        _ => null
      };

    public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
      => events.Select(this.Map);
  }
}