using System;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Posts.Models;
using Microservices.Services.Posts.Repositories;

namespace Microservices.Services.Posts.Commands.Handlers
{
  public class CreatePostHandler : ICommandHandler<CreatePost>
  {
    public readonly IPostsRepository _postsRepository;

    public CreatePostHandler(IPostsRepository postsRepository)
      => this._postsRepository = postsRepository;

    public async Task HandleAsync(CreatePost command)
    {
      var postId = Guid.NewGuid();
      var post = new Post(postId, command.Content);

      await this._postsRepository.AddAsync(post);
    }
  }
}