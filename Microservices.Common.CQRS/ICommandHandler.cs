using System.Threading.Tasks;

namespace Microservices.Common.CQRS
{
  public interface ICommandHandler<in TCommand> where TCommand : ICommand
  {
    Task HandleAsync(TCommand command);
  }
}