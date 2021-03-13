using System.Collections.Generic;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Core.Events;

namespace Microservices.Services.Posts.Application.Services
{
  public interface IEventMapper
  {
    IEvent Map(IDomainEvent @event);
    IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
  }
}