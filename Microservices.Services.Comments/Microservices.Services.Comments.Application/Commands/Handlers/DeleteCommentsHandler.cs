
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Comments.Core.Repositories;

namespace Microservices.Services.Comments.Application.Commands.Handlers
{
  public class DeleteCommentsHandler : ICommandHandler<DeleteComments>
  {
    private readonly ICommentsRepository _commentsRepository;

    public DeleteCommentsHandler(ICommentsRepository commentsRepository)
      => _commentsRepository = commentsRepository;

    public Task HandleAsync(DeleteComments command)
      => _commentsRepository.DeleteByPostIdAsync(command.PostId);
  }
}