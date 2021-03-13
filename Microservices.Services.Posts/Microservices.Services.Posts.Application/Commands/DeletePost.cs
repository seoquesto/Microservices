using System;
using Microservices.Common.CQRS;

namespace Microservices.Services.Posts.Application.Commands
{
  public class DeletePost : ICommand
  {
    public Guid Id { get; set; }
  }
}