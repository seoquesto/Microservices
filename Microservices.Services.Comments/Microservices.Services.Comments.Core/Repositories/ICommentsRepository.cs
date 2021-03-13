using System;
using System.Threading.Tasks;
using Microservices.Services.Comments.Core.Entities;

namespace Microservices.Services.Comments.Core.Repositories
{
  public interface ICommentsRepository
  {
    Task AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(Guid id);
  }
}