using System;
using Microservices.Common.CQRS;
using Microservices.Services.Comments.Application.Dto;

namespace Microservices.Services.Comments.Application.Queries
{
  public class GetComment : IQuery<CommentDto>
  {
    public Guid Id { get; set; }
  }
}