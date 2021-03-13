using System;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Common.MongoDb;
using Microservices.Services.Comments.Application.Dto;
using Microservices.Services.Comments.Application.Queries;
using Microservices.Services.Comments.Infrastructure.Mongo.Documents;

namespace Microservices.Services.Comments.Infrastructure.Mongo.QueryHandlers
{
  public sealed class GetCommentsQueryHandler : IQueryHandler<GetComments, PagedResult<CommentDto>>
  {
    private readonly IMongoRepository<CommentDocument, Guid> _repository;

    public GetCommentsQueryHandler(IMongoRepository<CommentDocument, Guid> repository)
      => this._repository = repository;

    public async Task<PagedResult<CommentDto>> HandleAsync(GetComments query)
    {
      var pageResult = await _repository.BrowseAsync(c => c.PostId == query.PostId, query);

      return pageResult?.Map(d => d.AsDto());
    }
  }
}