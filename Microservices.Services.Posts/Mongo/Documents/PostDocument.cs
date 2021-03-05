using System;
using Microservices.Common.Types;

namespace Microservices.Services.Posts.Mongo.Documents
{
  public class PostDocument : IIdentifiable<Guid>
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
  }
}