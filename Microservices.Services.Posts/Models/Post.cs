using System;
using Microservices.Common.Types;

namespace Microservices.Services.Posts.Models
{
  public class Post : IIdentifiable<Guid>
  {
    public Guid Id { get; protected set; }
    public string Content { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? ApprovedAt { get; protected set; }
    public bool IsApproved { get => this.ApprovedAt.HasValue; }

    protected Post()
    {
    }

    public Post(Guid id, string content)
    {
      // TODO: Better validation.
      // TODO: Specialized exceptions.
      if (id == Guid.Empty)
      {
        throw new Exception("Post Id cannot be empty!");
      }

      if (string.IsNullOrEmpty(content))
      {
        throw new Exception($"Content for post with id: {id} is required!");
      }

      this.Id = id;
      this.Content = content;
    }

    public Post(Guid id, string content, DateTime createdAt, DateTime? approvedAt) : this(id, content)
    {
      this.CreatedAt = DateTime.UtcNow;
      this.ApprovedAt = approvedAt;
    }

    public void Approve()
    {
      if (this.IsApproved)
      {
        // TODO: Specialized exceptions.
        throw new Exception("Post has already been approved!");
      }

      this.ApprovedAt = DateTime.UtcNow;
    }
  }
}