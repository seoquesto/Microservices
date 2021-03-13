using System;

namespace Microservices.Services.Comments.Application.Dto
{
  public class CommentDto
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
  }
}