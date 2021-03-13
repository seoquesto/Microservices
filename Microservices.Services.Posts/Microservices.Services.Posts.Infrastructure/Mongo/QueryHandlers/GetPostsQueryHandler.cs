using System;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Common.MongoDb;
using Microservices.Services.Posts.Application.Dto;
using Microservices.Services.Posts.Application.Queries;
using Microservices.Services.Posts.Infrastructure.Mongo.Documents;

namespace Microservices.Services.Posts.Infrastructure.Mongo.QueryHandlers
{
  public class GetPostsQueryHandler : IQueryHandler<GetPosts, PagedResult<PostDto>>
  {
    private readonly IMongoRepository<PostDocument, Guid> _repository;

    public GetPostsQueryHandler(IMongoRepository<PostDocument, Guid> repository)
      => this._repository = repository;

    public async Task<PagedResult<PostDto>> HandleAsync(GetPosts query)
    {
      var pageResult = await _repository.BrowseAsync(_ => true, query);

      return pageResult?.Map(d => d.AsDto());
    }
  }
}