namespace Microservices.Common.CQRS
{
  public abstract class PagedQueryBase : IPagedQuery, IQuery
  {
    public int Page { get; set; }
    public int Results { get; set; }
    public string OrderBy { get; set; }
    public string SortOrder { get; set; }
  }
}