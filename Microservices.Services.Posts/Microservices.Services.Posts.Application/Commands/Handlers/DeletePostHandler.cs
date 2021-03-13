using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Application.Events;
using Microservices.Services.Posts.Application.Services;
using Microservices.Services.Posts.Core.Repositories;

namespace Microservices.Services.Posts.Application.Commands.Handlers
{
  public class DeletePostHandler : ICommandHandler<DeletePost>
  {
    private readonly IPostsRepository _postsRepository;
    private readonly IMessageBroker _messageBroker;

    public DeletePostHandler(IPostsRepository postsRepository, IMessageBroker messageBroker)
      => (this._postsRepository, this._messageBroker) = (postsRepository, messageBroker);

    public async Task HandleAsync(DeletePost command)
    {
      // Check if user has permission to remove comment
      // owner and admin can remove a comment
      await this._postsRepository.DeleteAsync(command.Id);
      await this._messageBroker.PublishAsync(new PostRemoved { Id = command.Id });
    }
  }
}