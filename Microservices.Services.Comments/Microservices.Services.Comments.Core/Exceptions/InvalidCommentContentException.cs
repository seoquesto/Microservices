namespace Microservices.Services.Comments.Core.Exceptions
{
  public sealed class InvalidCommentContentException : DomainException
  {
    public override string ExceptionCode => "invalid_comment_content";
    public InvalidCommentContentException(string message) : base(message) { }
  }
}