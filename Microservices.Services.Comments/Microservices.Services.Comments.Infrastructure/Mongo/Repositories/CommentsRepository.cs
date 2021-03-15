using System;
using System.Threading.Tasks;
using Microservices.Common.MongoDb;
using Microservices.Services.Comments.Core.Entities;
using Microservices.Services.Comments.Core.Repositories;
using Microservices.Services.Comments.Infrastructure.Mongo.Documents;

namespace Microservices.Services.Comments.Infrastructure.Mongo.Repositories
{
  public sealed class CommentsRepository : ICommentsRepository
  {
    private readonly IMongoRepository<CommentDocument, Guid> _repository;

    public CommentsRepository(IMongoRepository<CommentDocument, Guid> repository)
      => this._repository = repository;

    public Task AddAsync(Comment comment)
      => this._repository.AddAsync(comment.AsDocument());

    public Task DeleteByCommentIdAsync(Guid commentId)
      => this._repository.DeleteOneAsync(c => c.Id == commentId);

    public Task DeleteByPostIdAsync(Guid postId)
      => this._repository.DeleteManyAsync(c => c.PostId == postId);

    public Task UpdateAsync(Comment comment)
    {
      return null;
      //  => this._postsRepository.Collection.ReplaceOneAsync(p => p.Id == post.Id && p.Version < post.Version,
      //           post.AsDocument());
    }
  }
}