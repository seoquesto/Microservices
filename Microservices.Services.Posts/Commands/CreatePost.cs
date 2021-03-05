using Microservices.Common.CQRS;

namespace Microservices.Services.Posts.Commands
{
  public class CreatePost : ICommand
  {
    public string Content { get; set; }
  }
}