using System;
using Microservices.Common.Api;
using Microservices.Common.CQRS;
using Microservices.Common.MongoDb;
using Microservices.Common.Prometheus;
using Microservices.Common.Types;
using Microservices.Services.Posts.Mongo.Documents;
using Microservices.Services.Posts.Mongo.Documents.Repositories;
using Microservices.Services.Posts.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Services.Posts
{
  public class Startup
  {
    public readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      this._configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddTransient<IPostsRepository, PostsRepository>();
      services.AddAppBuilder()
              .AddInMemoryCommandDispatcher()
              .AddCommandHandlers()
              .AddMongo()
              .AddMongoRepository<PostDocument, Guid>("Posts")
              .AddPrometheus();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UsePrometheus();
      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}