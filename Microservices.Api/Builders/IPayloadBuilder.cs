using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microservices.Api.Builders
{
  public interface IPayloadBuilder
  {
    Task<T> BuildFromJsonAsync<T>(HttpRequest request) where T : class, new();
  }
}