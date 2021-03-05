namespace Microservices.Common.CQRS
{
  public interface IPagedQuery : IQuery
  {
    int Page { get; }
    int Results { get; }
    string OrderBy { get; }
    string SortOrder { get; }
  }
}