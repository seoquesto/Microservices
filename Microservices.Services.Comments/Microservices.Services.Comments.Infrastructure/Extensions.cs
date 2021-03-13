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

namespace Microservices.Services.Comments.Infrastructure
{
  public static class Extensions
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient<ICommentsRepository, CommentsRepository>();
      serviceCollection
        .AddShellBuilder()
        .AddRabbitMq()
        .AddEventHandlers()
        .AddCommandHandlers()
        .AddQueryHandlers()
        .AddInMemoryCommandDispatcher()
        .AddInMemoryQueryDispatcher()
        .AddInMemoryEventDispatcher()
        .AddMongo()
        .AddMongoRepository<CommentDocument, Guid>("comments")
        .AddPrometheus()
        .Build();

      return serviceCollection;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
    {
      applicationBuilder
        .UsePrometheus()
        .UseShellBuilder()
        .UseRabbitMq()
        .SubscribeEvent<PostRemoved>();
      return applicationBuilder;
    }
  }
}