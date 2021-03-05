using System.Collections.Generic;

namespace Microservices.Common.CQRS
{
  public interface IFilter<TResult, in TQuery> where TQuery : IQuery
  {
    IEnumerable<TResult> Filter(IEnumerable<TResult> values, TQuery query);
  }
}