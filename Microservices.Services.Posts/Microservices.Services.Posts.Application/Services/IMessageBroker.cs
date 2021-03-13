using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Common.CQRS;

namespace Microservices.Services.Posts.Application.Services
{
  public interface IMessageBroker
  {
    Task PublishAsync(params IEvent[] events);
    Task PublishAsync(IEnumerable<IEvent> events);
  }
}