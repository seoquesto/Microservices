using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using MongoDB.Driver;

namespace Microservices.Common.MongoDb
{
  public interface IMongoRepository<TEntity, in TIdentifiable> where TEntity : IIdentifiable<TIdentifiable>
  {
    IMongoCollection<TEntity> Collection { get; }
    Task<TEntity> GetAsync(TIdentifiable id);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate,
      TQuery query) where TQuery : IPagedQuery;
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate);
    Task DeleteOneAsync(Expression<Func<TEntity, bool>> predicate);
    Task DeleteOneAsync(TIdentifiable id);
    Task DeleteManyAsync(TIdentifiable id);
    Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
  }
}