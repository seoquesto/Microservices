using System;
using Microservices.Common.CQRS;

namespace Microservices.Services.Comments.Application.Commands
{
  public class CreateComment : ICommand
  {
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; }
    public CreateComment(Guid commentId, Guid userId, Guid postId, string content)
    {
      this.CommentId = commentId == Guid.Empty ? Guid.NewGuid() : commentId;
      this.UserId = userId;
      this.PostId = postId;
      this.Content = content;
    }
  }
}