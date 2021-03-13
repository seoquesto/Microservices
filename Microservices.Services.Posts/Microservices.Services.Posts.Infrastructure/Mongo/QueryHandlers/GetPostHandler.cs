using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Application.Dto;
using Microservices.Services.Posts.Application.Queries;
using Microservices.Services.Posts.Core.Repositories;
using Microservices.Services.Posts.Infrastructure.Mongo.Documents;

namespace Microservices.Services.Posts.Infrastructure.Mongo.QueryHandlers
{
  public class GetPostHandler : IQueryHandler<GetPost, PostDto>
  {
    private readonly IPostsRepository _postsRepository;

    public GetPostHandler(IPostsRepository postsRepository)
      => this._postsRepository = postsRepository;

    public Task<PostDto> HandleAsync(GetPost query)
      => this._postsRepository.GetAsync(query.Id)?.AsDtoAsync();
  }
}