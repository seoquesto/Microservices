using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Identity.Api.Controllers
{
  [ApiController]
  [Route("")]
  public class HomeController : ControllerBase
  {
    [HttpGet]
    public IActionResult Get() => Content("Posts service is up and running...");
  }
}