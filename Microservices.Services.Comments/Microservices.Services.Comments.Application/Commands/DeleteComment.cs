using System;
using Microservices.Common.CQRS;

namespace Microservices.Services.Comments.Application.Commands
{
  public class DeleteComment : ICommand
  {
    public Guid CommentId { get; set; }
  }
}