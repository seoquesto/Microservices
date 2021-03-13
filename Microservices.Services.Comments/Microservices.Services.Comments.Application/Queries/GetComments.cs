using System;
using Microservices.Common.CQRS;
using Microservices.Services.Comments.Application.Dto;

namespace Microservices.Services.Comments.Application.Queries
{
  public class GetComments : PagedQueryBase, IQuery<PagedResult<CommentDto>>
  {
    public Guid PostId { get; set; }
  }
}