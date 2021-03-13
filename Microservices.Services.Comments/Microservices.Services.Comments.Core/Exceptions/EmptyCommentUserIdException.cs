namespace Microservices.Services.Comments.Core.Exceptions
{
  public sealed class EmptyCommentUserIdException : DomainException
  {
    public override string ExceptionCode => "empty_comment_user_id";
    public EmptyCommentUserIdException(string message) : base(message) { }
  }
}