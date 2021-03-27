using System;
using Microservices.Common.CQRS;

namespace Microservices.Services.Comments.Application.Commands
{
  public class DeleteComments : ICommand
  {
    public Guid PostId { get; set; }
  }
}