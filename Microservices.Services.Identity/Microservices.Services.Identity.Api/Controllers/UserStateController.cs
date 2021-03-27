using Microservices.Services.Identity.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
namespace Microservices.Services.Identity.Api.Controllers
{
  [ApiController]
  [Route("api/v1/users/state")]
  public class UsersController : ControllerBase
  {
    [HttpGet]
    public IActionResult GetAsync(Guid guid)
    {
      return Ok(JsonSerializer.Serialize(new UserStateDto { State = "valid" }));
    }
  }
}