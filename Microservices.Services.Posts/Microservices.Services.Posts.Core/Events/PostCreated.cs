using Microservices.Services.Posts.Core.Entities;

namespace Microservices.Services.Posts.Core.Events
{
  public class PostCreated : IDomainEvent
  {
    public Post Post { get; }
    public PostCreated(Post post) => this.Post = post;
  }
}