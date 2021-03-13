namespace Microservices.Services.Posts.Core.Exceptions
{
  public sealed class PostAlreadyApprovedException : DomainException
  {
    public override string ExceptionCode => "post_already_approved";
    public PostAlreadyApprovedException() : base("Post has already been approved") { }
  }
}