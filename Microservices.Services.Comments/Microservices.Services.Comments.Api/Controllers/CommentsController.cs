using System;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Comments.Application.Commands;
using Microservices.Services.Comments.Application.Dto;
using Microservices.Services.Comments.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Comments.Api.Controllers
{
  [ApiController]
  [Route("api/v1/comments")]
  public class CommentsController : ControllerBase
  {
    public readonly ICommandDispatcher _commandDispatcher;
    public readonly IQueryDispatcher _queryDispatcher;

    public CommentsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        => (this._commandDispatcher, this._queryDispatcher) = (commandDispatcher, queryDispatcher);

    [HttpPost]
    public async Task<IActionResult> CreateCommentAsync([FromBody] CreateComment command)
    {
      await this._commandDispatcher.SendAsync(command);
      return Created($"api/v1/comments/{command.CommentId.ToString()}", new object());
    }

    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentsAsync([FromRoute] Guid commentId)
    {
      var post = await _queryDispatcher.QueryAsync<CommentDto>(new GetComment { Id = commentId });
      return Ok(post);
    }

    [HttpGet]
    public async Task<IActionResult> GetCommentsAsync([FromQuery] GetComments query)
    {
      var posts = await _queryDispatcher.QueryAsync<PagedResult<CommentDto>>(query);
      return Ok(posts);
    }
  }
}