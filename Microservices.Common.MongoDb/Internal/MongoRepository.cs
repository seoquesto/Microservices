using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Microservices.Common.MongoDb.Internal
{
  internal class MongoRepository<TEntity, TIdentifiable> : IMongoRepository<TEntity, TIdentifiable>
    where TEntity : IIdentifiable<TIdentifiable>
  {
    public IMongoCollection<TEntity> Collection { get; }

    public MongoRepository(IMongoDatabase database, string collectionName)
      => this.Collection = database.GetCollection<TEntity>(collectionName);

    public Task<TEntity> GetAsync(TIdentifiable id)
      => GetAsync(e => e.Id.Equals(id));

    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
      => Collection.Find(predicate).SingleOrDefaultAsync();

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
      => await Collection.Find(predicate).ToListAsync();

    public Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate,
      TQuery query) where TQuery : IPagedQuery
      => Collection.AsQueryable().Where(predicate).PaginateAsync(query);

    public Task AddAsync(TEntity entity)
      => Collection.InsertOneAsync(entity);

    public Task UpdateAsync(TEntity entity)
      => UpdateAsync(entity, e => e.Id.Equals(entity.Id));

    public Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
      => Collection.ReplaceOneAsync(predicate, entity);

    public Task DeleteOneAsync(TIdentifiable id)
      => DeleteOneAsync(e => e.Id.Equals(id));

    public Task DeleteOneAsync(Expression<Func<TEntity, bool>> predicate)
      => Collection.DeleteOneAsync(predicate);

    public Task DeleteManyAsync(TIdentifiable id)
      => DeleteManyAsync(e => e.Id.Equals(id));

    public Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
      => Collection.DeleteManyAsync(predicate);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
      => Collection.Find(predicate).AnyAsync();
  }
}