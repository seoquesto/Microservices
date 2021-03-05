using System.Collections.Generic;

namespace Microservices.Common.CQRS
{
  public interface IPagedFilter<TResult, in TQuery> where TQuery : IQuery
  {
    PagedResult<TResult> Filter(IEnumerable<TResult> values, TQuery query);
  }
}