namespace Microservices.Services.Comments.Core.Exceptions
{
  public sealed class EmptyCommentPostIdException : DomainException
  {
    public override string ExceptionCode => "empty_comment_post_id";
    public EmptyCommentPostIdException(string message) : base(message) { }
  }
}