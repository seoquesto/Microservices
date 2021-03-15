using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Services.Comments.Core.Repositories;

namespace Microservices.Services.Comments.Application.Events.External.Handlers
{
  public class PostRemovedEventHandler : IEventHandler<PostRemoved>
  {
    private readonly ICommentsRepository _commentsRepository;
    public PostRemovedEventHandler(ICommentsRepository commentsRepository)
      => this._commentsRepository = commentsRepository;

    public Task HandleAsync(PostRemoved @event)
      => this._commentsRepository.DeleteByPostIdAsync(@event.Id);
  }
}