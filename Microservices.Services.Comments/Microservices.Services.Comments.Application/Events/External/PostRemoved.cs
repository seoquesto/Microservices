using System;
using Microservices.Common.CQRS;
using Microservices.Common.RabbitMq;

namespace Microservices.Services.Comments.Application.Events.External
{
  [Message("posts")]
  public class PostRemoved : IEvent
  {
    public Guid Id { get; set; }
  }
}