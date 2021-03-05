using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Posts.Controllers
{
  [ApiController]
  [Route("")]
  public class HomeController : ControllerBase
  {
    [HttpGet]
    public ActionResult Get() => Ok("Posts service is running!");
  }
}
