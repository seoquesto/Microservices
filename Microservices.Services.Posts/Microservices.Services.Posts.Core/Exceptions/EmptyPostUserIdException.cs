namespace Microservices.Services.Posts.Core.Exceptions
{
  public sealed class EmptyPostUserIdException : DomainException
  {
    public override string ExceptionCode => "empty_post_user_id";
    public EmptyPostUserIdException(string message) : base(message) { }
  }
}