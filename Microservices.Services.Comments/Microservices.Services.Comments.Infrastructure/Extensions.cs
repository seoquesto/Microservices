using System;
using Microservices.Common.Shell;
using Microservices.Common.CQRS;
using Microservices.Common.MongoDb;
using Microservices.Services.Comments.Core.Repositories;
using Microservices.Services.Comments.Infrastructure.Mongo.Documents;
using Microservices.Services.Comments.Infrastructure.Mongo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Common.Prometheus;
using Microservices.Common.RabbitMq;
using Microservices.Services.Comments.Application.Events.External;
using Microservices.Services.Comments.Application.Commands;
using Microservices.Services.Comments.Infrastructure.Exceptions;
using Microservices.Common.WebApi;
using Microservices.Common.AppMetrics;
using Microservices.Services.Comments.Infrastructure.Metrics;

namespace Microservices.Services.Comments.Infrastructure
{
  public static class Extensions
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient<ICommentsRepository, CommentsRepository>()
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
        .AddMongoRepository<CommentDocument, Guid>("comments")
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
        .SubscribeEvent<PostRemoved>()
        .SubscribeCommand<CreateComment>();
      return applicationBuilder;
    }
  }
}