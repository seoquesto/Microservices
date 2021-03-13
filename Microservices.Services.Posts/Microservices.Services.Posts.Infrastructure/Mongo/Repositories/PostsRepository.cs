using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Common.MongoDb;
using Microservices.Services.Posts.Core.Entities;
using Microservices.Services.Posts.Core.Repositories;
using Microservices.Services.Posts.Infrastructure.Mongo.Documents;

namespace Microservices.Services.Posts.Infrastructure.Mongo.Repositories
{
  public sealed class PostsRepository : IPostsRepository
  {
    private readonly IMongoRepository<PostDocument, Guid> _postsRepository;

    public PostsRepository(IMongoRepository<PostDocument, Guid> postsRepository)
      => this._postsRepository = postsRepository;

    public Task AddAsync(Post post)
      => this._postsRepository.AddAsync(post.AsDocument());

    public Task DeleteAsync(Guid id)
      => this._postsRepository.DeleteAsync(id);

    public Task<IEnumerable<Post>> GetAllAsync()
      => this._postsRepository.FindAsync(_ => true).AsEntityAsync();

    public Task<Post> GetAsync(Guid id)
      => this._postsRepository.GetAsync(id)?.AsEntityAsync();

    public Task UpdateAsync(Post post)
    {
      return null;
      //  => this._postsRepository.Collection.ReplaceOneAsync(p => p.Id == post.Id && p.Version < post.Version,
      //           post.AsDocument());
    }
  }
}