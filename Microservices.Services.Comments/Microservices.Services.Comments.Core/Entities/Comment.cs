using System;
using Microservices.Services.Comments.Core.Events;
using Microservices.Services.Comments.Core.Exceptions;

// TODO: Better validation.
// TODO: Specialized exceptions.
namespace Microservices.Services.Comments.Core.Entities
{
  public class Comment : AggregateRoot
  {
    public Guid UserId { get; protected set; }
    public Guid PostId { get; protected set; }
    public string Content { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? ApprovedAt { get; protected set; }
    public bool IsApproved { get => this.ApprovedAt.HasValue; }

    protected Comment() { }

    public Comment(Guid id, Guid userId, Guid postId ,string content)
      : this(id, userId, postId, content, null, null, null) { }

    public Comment(Guid id, Guid userId,Guid postId, string content, int? version, DateTime? createdAt, DateTime? approvedAt)
    {
      if (userId == Guid.Empty)
      {
        throw new EmptyCommentUserIdException($"User id of the comment cannot be empty.");
      }

       if (postId == Guid.Empty)
      {
        throw new EmptyCommentPostIdException($"Post id for the comment with id {id} cannot be empty.");
      }

      if (string.IsNullOrEmpty(content))
      {
        throw new InvalidCommentContentException($"Content of the comment is required.");
      }

      this.Id = id;
      this.UserId = userId;
      this.Content = content;
      this.PostId = postId;
      this.Version = version ?? 0;
      this.CreatedAt = createdAt ?? DateTime.UtcNow;
    }

    public void Approve()
    {
      if (this.IsApproved)
      {
        throw new CommentAlreadyApprovedException();
      }

      this.ApprovedAt = DateTime.UtcNow;
    }

    public static Comment Create(Guid id, Guid userId, Guid postId, string content)
    {
      var comment = new Comment(id, userId, postId, content);
      comment.AddEvent(new CommentCreated(comment));
      return comment;
    }
  }
}