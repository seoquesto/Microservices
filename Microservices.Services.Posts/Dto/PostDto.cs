using System;

namespace Microservices.Services.Posts.Dto
{
  public class PostDto
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool IsApproved { get => this.ApprovedAt.HasValue; }
  }
}