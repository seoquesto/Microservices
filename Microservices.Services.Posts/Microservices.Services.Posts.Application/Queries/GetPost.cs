using System;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Application.Dto;

namespace Microservices.Services.Posts.Application.Queries
{
  public class GetPost : IQuery<PostDto>
  {
    public Guid Id { get; set; }
  }
}