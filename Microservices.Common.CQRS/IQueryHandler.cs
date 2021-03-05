using System.Threading.Tasks;

namespace Microservices.Common.CQRS
{
  public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
  {
    Task<TResult> HandleAsync(TQuery query);
  }
}