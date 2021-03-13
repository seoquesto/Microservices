using System;

namespace Microservices.Services.Posts.Application.Dto
{
  public class PostDto
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
  }
}