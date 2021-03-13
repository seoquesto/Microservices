using System;
using Microservices.Common.Shell;
using Microservices.Common.CQRS;
using Microservices.Common.MongoDb;
using Microservices.Common.WebApi;
using Microservices.Services.Posts.Core.Repositories;
using Microservices.Services.Posts.Infrastructure.Mongo.Documents;
using Microservices.Services.Posts.Infrastructure.Mongo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Common.Prometheus;
using Microservices.Services.Posts.Infrastructure.Exceptions;
using Microservices.Services.Posts.Application.Services;
using Microservices.Services.Posts.Infrastructure.Services;
using Microservices.Common.RabbitMq;

namespace Microservices.Services.Posts.Infrastructure
{
  public static class Extensions
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
      serviceCollection
        .AddTransient<IPostsRepository, PostsRepository>()
        .AddSingleton<IEventMapper, EventMapper>()
        .AddTransient<IMessageBroker, MessageBroker>();

      serviceCollection
        .AddShellBuilder()
        .AddWebApi()
        .AddRabbitMq()
        .AddErrorHandler<ExceptionToResponseMapper>()
        .AddCommandHandlers()
        .AddQueryHandlers()
        .AddEventHandlers()
        .AddInMemoryCommandDispatcher()
        .AddInMemoryQueryDispatcher()
        .AddInMemoryEventDispatcher()
        .AddMongo()
        .AddMongoRepository<PostDocument, Guid>("posts")
        .AddPrometheus()
        .Build();

      return serviceCollection;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
      => applicationBuilder
        .UsePrometheus()
        .UseErrorHandler()
        .UseShellBuilder();
  }
}