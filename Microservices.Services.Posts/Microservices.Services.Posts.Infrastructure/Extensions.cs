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
using Microservices.Services.Posts.Application.Commands;
using Microservices.Common.AppMetrics;
using Microservices.Services.Posts.Infrastructure.Metrics;

namespace Microservices.Services.Posts.Infrastructure
{
  public static class Extensions
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
      serviceCollection.AddControllers();
      serviceCollection
        .AddTransient<IPostsRepository, PostsRepository>()
        .AddSingleton<IEventMapper, EventMapper>()
        .AddTransient<IMessageBroker, MessageBroker>()
        .AddHostedService<MetricsJob>();

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
        .AddMetrics()
        .Build();

      return serviceCollection;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
    {
      applicationBuilder
        .UseRouting()
        .UseEndpoints(endpoints => endpoints.MapControllers())
        .UsePrometheus()
        .UseMetrics()
        .UseErrorHandler()
        .UseShellBuilder()
        .UseRabbitMq()
        .SubscribeCommand<CreatePost>();
      return applicationBuilder;
    }
  }
}