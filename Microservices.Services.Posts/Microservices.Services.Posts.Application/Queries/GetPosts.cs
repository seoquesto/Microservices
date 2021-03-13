using Microservices.Common.CQRS;
using Microservices.Services.Posts.Application.Dto;

namespace Microservices.Services.Posts.Application.Queries
{
  public class GetPosts : PagedQueryBase, IQuery<PagedResult<PostDto>>
  {
  }
}