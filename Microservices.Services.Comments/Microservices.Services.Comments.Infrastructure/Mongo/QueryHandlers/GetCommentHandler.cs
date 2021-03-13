using System;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Common.MongoDb;
using Microservices.Services.Comments.Application.Dto;
using Microservices.Services.Comments.Application.Queries;
using Microservices.Services.Comments.Infrastructure.Mongo.Documents;

namespace Microservices.Services.Comments.Infrastructure.Mongo.QueryHandlers
{
  public sealed class GetCommentHandler : IQueryHandler<GetComment, CommentDto>
  {
    private readonly IMongoRepository<CommentDocument, Guid> _repository;

    public GetCommentHandler(IMongoRepository<CommentDocument, Guid> repository)
      => this._repository = repository;

    public Task<CommentDto> HandleAsync(GetComment query)
      => this._repository.GetAsync(x => x.Id == query.Id).AsDtoAsync();
  }
}