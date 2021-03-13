using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Comments.Core.Entities;
using Microservices.Services.Comments.Core.Repositories;

namespace Microservices.Services.Comments.Application.Commands.Handlers
{
  public sealed class CreateCommentHandler : ICommandHandler<CreateComment>
  {
    private readonly ICommentsRepository _commentRepository;

    public CreateCommentHandler(ICommentsRepository commentRepository)
      => (this._commentRepository) = (commentRepository);

    public async Task HandleAsync(CreateComment command)
    {
      var comment = Comment.Create(command.CommentId, command.UserId, command.PostId, command.Content);
      await this._commentRepository.AddAsync(comment);
    }
  }
}