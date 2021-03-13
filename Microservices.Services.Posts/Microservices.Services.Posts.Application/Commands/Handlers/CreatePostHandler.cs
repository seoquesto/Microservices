using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Application.Services;
using Microservices.Services.Posts.Core.Entities;
using Microservices.Services.Posts.Core.Repositories;

namespace Microservices.Services.Posts.Application.Commands.Handlers
{
  public sealed class CreatePostHandler : ICommandHandler<CreatePost>
  {
    private readonly IPostsRepository _postsRepository;
    private readonly IEventMapper _eventMapper;
    private readonly IMessageBroker _messageBroker;

    public CreatePostHandler(IPostsRepository postsRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
      => (this._postsRepository, this._eventMapper, this._messageBroker) = (postsRepository, eventMapper, messageBroker);

    public async Task HandleAsync(CreatePost command)
    {
      var post = Post.Create(command.PostId, command.UserId, command.Content);
      await this._postsRepository.AddAsync(post);
      var events = this._eventMapper.MapAll(post.Events);
      await this._messageBroker.PublishAsync(events);
    }
  }
}