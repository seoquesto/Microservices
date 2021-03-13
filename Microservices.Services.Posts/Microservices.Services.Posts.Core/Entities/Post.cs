using System;
using Microservices.Services.Posts.Core.Events;
using Microservices.Services.Posts.Core.Exceptions;

// TODO: Better validation.
// TODO: Specialized exceptions.
namespace Microservices.Services.Posts.Core.Entities
{
  public class Post : AggregateRoot
  {
    public Guid UserId { get; protected set; }
    public string Content { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? ApprovedAt { get; protected set; }
    public bool IsApproved { get => this.ApprovedAt.HasValue; }

    protected Post() { }

    public Post(Guid id, Guid userId, string content)
      : this(id, userId, content, null, null, null) { }

    public Post(Guid id, Guid userId, string content, int? version, DateTime? createdAt, DateTime? approvedAt)
    {
      if (userId == Guid.Empty)
      {
        throw new EmptyPostUserIdException($"User id of the post cannot be empty.");
      }

      if (string.IsNullOrEmpty(content))
      {
        throw new InvalidPostContentException($"Content of the post is required.");
      }

      this.Id = id;
      this.UserId = userId;
      this.Content = content;
      this.Version = version ?? 0;
      this.CreatedAt = createdAt ?? DateTime.UtcNow;
    }

    public void Approve()
    {
      if (this.IsApproved)
      { 
        throw new PostAlreadyApprovedException();
      }

      this.ApprovedAt = DateTime.UtcNow;
    }

    public static Post Create(Guid id, Guid userId, string content)
    {
      var post = new Post(id, userId, content);
      post.AddEvent(new PostCreated(post));
      return post;
    }
  }
}