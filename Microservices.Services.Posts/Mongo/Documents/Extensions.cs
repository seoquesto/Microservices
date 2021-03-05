using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Services.Posts.Models;
using System.Linq;

namespace Microservices.Services.Posts.Mongo.Documents
{
  public static class Extensions
  {
    public static PostDocument AsDocument(this Post post)
      => new PostDocument
      {
        Id = post.Id,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        ApprovedAt = post.ApprovedAt
      };

    public static async Task<Post> AsEntityAsync(this Task<PostDocument> document)
      => (await document).AsEntity();

    public static async Task<IEnumerable<Post>> AsEntityAsync(this Task<IReadOnlyList<PostDocument>> document)
      => (await document).Select(x => x.AsEntity());

    public static Post AsEntity(this PostDocument document)
      => new Post(document.Id, document.Content, document.CreatedAt, document.ApprovedAt);
  }
}