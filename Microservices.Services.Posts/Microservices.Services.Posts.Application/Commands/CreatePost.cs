using System;
using Microservices.Common.CQRS;

namespace Microservices.Services.Posts.Application.Commands
{
  public class CreatePost : ICommand
  {
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public CreatePost(Guid postId, Guid userId, string content)
    {
      this.PostId = postId == Guid.Empty ? Guid.NewGuid() : postId;
      this.UserId = userId;
      this.Content = content;
    }
  }
}