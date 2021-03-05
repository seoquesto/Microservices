using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Services.Posts.Models;

namespace Microservices.Services.Posts.Repositories
{
  public interface IPostsRepository
  {
    Task AddAsync(Post post);
    Task<Post> GetAsync(Guid id);
    Task<IEnumerable<Post>> GetAllAsync();
    Task UpdateAsync(Post post);
    Task DeleteAsync(Guid id);
  }
}