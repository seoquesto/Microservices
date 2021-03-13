using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Common.RabbitMq;
using Microservices.Services.Posts.Application.Services;
using Microsoft.Extensions.Logging;

namespace Microservices.Services.Posts.Infrastructure.Services
{
  public class MessageBroker : IMessageBroker
  {
    private readonly IBusPublisher _busPublisher;
    private readonly ILogger<IMessageBroker> _logger;

    public MessageBroker(IBusPublisher busPublisher, ILogger<IMessageBroker> logger)
    {
      this._busPublisher = busPublisher;
      this._logger = logger;
    }
    public Task PublishAsync(params IEvent[] events) => PublishAsync(events?.AsEnumerable());

    public async Task PublishAsync(IEnumerable<IEvent> events)
    {
      if (events is null)
      {
        return;
      }

      foreach (var @event in events)
      {
        if (@event is null)
        {
          continue;
        }

        var messageId = Guid.NewGuid().ToString("N");
        _logger.LogTrace($"Publishing integration event: {@event.GetType().Name} [id: '{messageId}'].");

        await _busPublisher.PublishAsync(@event, messageId);
      }
    }
  }
}