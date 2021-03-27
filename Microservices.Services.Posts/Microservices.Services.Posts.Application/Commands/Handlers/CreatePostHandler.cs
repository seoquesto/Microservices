using System;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Application.Services;
using Microservices.Services.Posts.Application.Services.Clients;
using Microservices.Services.Posts.Core.Entities;
using Microservices.Services.Posts.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Microservices.Services.Posts.Application.Commands.Handlers
{
  public sealed class CreatePostHandler : ICommandHandler<CreatePost>
  {
    private readonly IPostsRepository _postsRepository;
    private readonly IEventMapper _eventMapper;
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<CreatePostHandler> _logger;
    private readonly IIdentityServiceClient _identityClient;

    public CreatePostHandler(
      IPostsRepository postsRepository,
      IEventMapper eventMapper,
      IMessageBroker messageBroker,
      ILogger<CreatePostHandler> logger,
      IIdentityServiceClient identityClient)
    => (
      this._postsRepository,
      this._eventMapper,
      this._messageBroker,
      this._logger,
      this._identityClient
      ) = (
        postsRepository,
        eventMapper,
        messageBroker,
        logger,
        identityClient
      );

    public async Task HandleAsync(CreatePost command)
    {
      this.CreatePostLog(command.PostId);

      // TODO: Real case
      var userId = Guid.NewGuid();
      var userStatus = await _identityClient.GetUserState(userId);


      if (!userStatus.State.Equals("valid", StringComparison.InvariantCultureIgnoreCase))
      {
        throw new Exception("User status for userId: " + userId.ToString() + " is not valid");
      }

      var post = Post.Create(command.PostId, command.UserId, command.Content);
      await this._postsRepository.AddAsync(post);
      var events = this._eventMapper.MapAll(post.Events);
      await this._messageBroker.PublishAsync(events);
      this.CreatedPostLog(command.PostId);
    }

    internal void CreatePostLog(Guid postId) => this._logger.LogInformation("Creating post with id: " + postId.ToString());
    internal void CreatedPostLog(Guid postId) => this._logger.LogInformation("Created post with id: " + postId.ToString());
  }
}