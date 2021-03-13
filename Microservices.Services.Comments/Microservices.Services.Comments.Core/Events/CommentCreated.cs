using Microservices.Services.Comments.Core.Entities;

namespace Microservices.Services.Comments.Core.Events
{
  public class CommentCreated : IDomainEvent
  {
    public Comment Comment { get; }
    public CommentCreated(Comment comment) => this.Comment = comment;
  }
}