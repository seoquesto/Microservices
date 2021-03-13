namespace Microservices.Services.Comments.Core.Exceptions
{
  public sealed class InvalidAggregateIdException : DomainException
  {
    public override string ExceptionCode => "invalid_aggregate_id";
    public InvalidAggregateIdException() : base("Aggregated id cannot be empty.") { }
  }
}