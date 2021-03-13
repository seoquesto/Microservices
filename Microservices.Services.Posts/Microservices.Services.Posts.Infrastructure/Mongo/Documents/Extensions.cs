using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Services.Posts.Application.Dto;
using Microservices.Services.Posts.Core.Entities;

namespace Microservices.Services.Posts.Infrastructure.Mongo.Documents
{
  public static class Extensions
  {
    public static Post AsEntity(this PostDocument document)
        => new Post(
          id: document.Id,
          userId: document.UserId,
          content: document.Content,
          version: document.Version,
          createdAt: document.CreatedAt,
          approvedAt: document.ApprovedAt);

    public static PostDocument AsDocument(this Post entity)
        => new PostDocument
        {
          Id = entity.Id,
          UserId = entity.UserId,
          Version = entity.Version,
          Content = entity.Content,
          CreatedAt = entity.CreatedAt,
          ApprovedAt = entity.ApprovedAt,
        };

    public static PostDto AsDto(this PostDocument document)
        => new PostDto
        {
          Id = document.Id,
          UserId = document.UserId,
          Content = document.Content,
          CreatedAt = document.CreatedAt,
          ApprovedAt = document.ApprovedAt,
        };

    public static async Task<PostDto> AsDtoAsync(this Task<Post> task)
    {
      var post = await task;
      return post is { } ? new PostDto
      {
        Id = post.Id,
        UserId = post.UserId,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        ApprovedAt = post.ApprovedAt,
      } : null;
    }

    public static async Task<Post> AsEntityAsync(this Task<PostDocument> document)
         => (await document).AsEntity();

    public static async Task<IEnumerable<Post>> AsEntityAsync(this Task<IReadOnlyList<PostDocument>> document)
      => (await document).Select(x => x.AsEntity());
  }
}