using System;
using Microservices.Common.MongoDb;

namespace Microservices.Services.Comments.Infrastructure.Mongo.Documents
{
  public class CommentDocument : IIdentifiable<Guid>
  {
    public Guid Id { get; set; }
    public int Version { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
  }
}