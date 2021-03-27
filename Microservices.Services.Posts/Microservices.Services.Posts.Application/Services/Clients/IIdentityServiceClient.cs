using System;
using System.Threading.Tasks;
using Microservices.Services.Posts.Application.Dto;

namespace Microservices.Services.Posts.Application.Services.Clients
{
  public interface IIdentityServiceClient
  {
    Task<UserStateDto> GetUserState(Guid id);
  }
}