using System;
using Microservices.Common.CQRS;

namespace Microservices.Services.Posts.Application.Events
{
  public class PostRemoved : IEvent
  {
    public Guid Id { get; set; }
  }
}