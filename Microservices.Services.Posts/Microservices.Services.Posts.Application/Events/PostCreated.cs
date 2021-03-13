using System;
using Microservices.Common.CQRS;

namespace Microservices.Services.Posts.Application.Events
{
  public class PostCreated : IEvent
  {
    public Guid Id { get; set; }
  }
}