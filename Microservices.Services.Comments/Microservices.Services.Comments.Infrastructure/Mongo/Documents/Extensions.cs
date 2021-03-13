using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Services.Comments.Application.Dto;
using Microservices.Services.Comments.Core.Entities;

namespace Microservices.Services.Comments.Infrastructure.Mongo.Documents
{
  public static class Extensions
  {
    public static Comment AsEntity(this CommentDocument document)
        => new Comment(
          id: document.Id,
          userId: document.UserId,
          postId: document.PostId,
          content: document.Content,
          version: document.Version,
          createdAt: document.CreatedAt,
          approvedAt: document.ApprovedAt);

    public static CommentDocument AsDocument(this Comment entity)
        => new CommentDocument
        {
          Id = entity.Id,
          UserId = entity.UserId,
          PostId = entity.PostId,
          Version = entity.Version,
          Content = entity.Content,
          CreatedAt = entity.CreatedAt,
          ApprovedAt = entity.ApprovedAt,
        };

    public static CommentDto AsDto(this CommentDocument document)
        => new CommentDto
        {
          Id = document.Id,
          UserId = document.UserId,
          PostId = document.PostId,
          Content = document.Content,
          CreatedAt = document.CreatedAt,
          ApprovedAt = document.ApprovedAt,
        };

    public static CommentDto AsDto(this Comment comment)
      => new CommentDto
      {
        Id = comment.Id,
        UserId = comment.UserId,
        PostId = comment.PostId,
        Content = comment.Content,
        CreatedAt = comment.CreatedAt,
        ApprovedAt = comment.ApprovedAt,
      };

    public static async Task<CommentDto> AsDtoAsync(this Task<CommentDocument> task)
    => (await task)?.AsDto();

    public static async Task<CommentDto> AsDtoAsync(this Task<Comment> task)
      => (await task)?.AsDto();

    public static async Task<Comment> AsEntityAsync(this Task<CommentDocument> document)
         => (await document).AsEntity();

    public static async Task<IEnumerable<Comment>> AsEntityAsync(this Task<IReadOnlyList<CommentDocument>> document)
      => (await document).Select(x => x.AsEntity());
  }
}