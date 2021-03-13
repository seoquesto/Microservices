using System;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Application.Commands;
using Microservices.Services.Posts.Application.Dto;
using Microservices.Services.Posts.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Posts.Api.Controllers
{
  [ApiController]
  [Route("api/v1/posts")]
  public class PostsController : ControllerBase
  {
    public readonly ICommandDispatcher _commandDispatcher;
    public readonly IQueryDispatcher _queryDispatcher;

    public PostsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        => (this._commandDispatcher, this._queryDispatcher) = (commandDispatcher, queryDispatcher);

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePost command)
    {
      await this._commandDispatcher.SendAsync(command);
      return Created($"api/v1/posts/{command.PostId.ToString()}", new object());
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid postId)
    {
      var postDto = await _queryDispatcher.QueryAsync<PostDto>(new GetPost { Id = postId });
      return Ok(postDto);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid postId)
    {
      await this._commandDispatcher.SendAsync(new DeletePost { Id = postId });
      return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] GetPosts query)
    {
      var posts = await _queryDispatcher.QueryAsync<PagedResult<PostDto>>(query);
      return Ok(posts);
    }
  }
}