using System.Threading.Tasks;

namespace Microservices.Common.CQRS
{
  public interface IQueryDispatcher
  {
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>;
  }
}