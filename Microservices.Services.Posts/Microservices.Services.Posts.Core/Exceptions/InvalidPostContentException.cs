namespace Microservices.Services.Posts.Core.Exceptions
{
  public sealed class InvalidPostContentException : DomainException
  {
    public override string ExceptionCode => "invalid_post_content";
    public InvalidPostContentException(string message) : base(message) { }
  }
}