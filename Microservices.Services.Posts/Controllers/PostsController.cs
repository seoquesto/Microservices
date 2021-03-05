using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Posts.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class PostsController : ControllerBase
  {
    public readonly ICommandDispatcher _commandDispatcher;

    public PostsController(ICommandDispatcher commandDispatcher)
    {
      this._commandDispatcher = commandDispatcher;
    }

    public Task CreatePostAsync(CreatePost command)
      => _commandDispatcher.SendAsync(command);
  }
}