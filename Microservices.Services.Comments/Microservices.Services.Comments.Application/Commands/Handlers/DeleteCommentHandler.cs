using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Comments.Core.Repositories;

namespace Microservices.Services.Comments.Application.Commands.Handlers
{
  public class DeleteCommentHandler : ICommandHandler<DeleteComment>
  {
    private readonly ICommentsRepository _commentsRepository;

    public DeleteCommentHandler(ICommentsRepository commentsRepository)
      => _commentsRepository = commentsRepository;

    public Task HandleAsync(DeleteComment command)
      => _commentsRepository.DeleteByCommentIdAsync(command.CommentId);
  }
}