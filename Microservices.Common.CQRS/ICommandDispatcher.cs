using System.Threading.Tasks;

namespace Microservices.Common.CQRS
{
  public interface ICommandDispatcher
  {
    Task SendAsync<T>(T command) where T : class, ICommand;
  }
}