using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Comments.Api.Controllers
{
  [ApiController]
  [Route("")]
  public class HomeController : ControllerBase
  {
    [HttpGet]
    public IActionResult Get() => Content("Comments service is up and running...");
  }
}
