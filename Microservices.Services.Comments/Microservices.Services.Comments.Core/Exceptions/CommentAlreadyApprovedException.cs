namespace Microservices.Services.Comments.Core.Exceptions
{
  public sealed class CommentAlreadyApprovedException : DomainException
  {
    public override string ExceptionCode => "comment_already_approved";
    public CommentAlreadyApprovedException() : base("Comment has already been approved") { }
  }
}