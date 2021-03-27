using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Microservices.Common.Shell;
using Microservices.Common.WebApi;
using Microservices.Api.Middleware;
using Microservices.Api.Options;
using Microservices.Common.RabbitMq;
using Microservices.Api.Builders;
using Microservices.Api.Correlation;

namespace Microservices.Api
{
  public class Program
  {
    public static Task Main(string[] args)
    => WebHost.CreateDefaultBuilder(args)
      .ConfigureAppConfiguration((hostingContext, config) =>
      {
        config
            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
      })
      .ConfigureServices(services =>
      {
        var provider = services.BuildServiceProvider();
        var config = provider.GetService<IConfiguration>();
        var asyncRoutesOptions = new AsyncRoutesOptions();
        config.GetSection("AsyncRoutes").Bind(asyncRoutesOptions);

        services.AddSingleton<AsyncRoutesOptions>(asyncRoutesOptions);
        services.AddTransient<AsyncRoutesMiddleware>();
        services.AddSingleton<IPayloadBuilder, PayloadBuilder>();
        services.AddSingleton<ICorrelationContextBuilder, CorrelationContextBuilder>();
        services.AddOcelot()
                .AddPolly();
        services.AddShellBuilder()
                .AddWebApi()
                .AddRabbitMq()
                .Build();

      })
      .Configure(appBuilder =>
      {
        appBuilder.MapWhen(ctx => ctx.Request.Path == "/", config =>
        {
          config.Use((ctx, next) => ctx.Response.WriteAsync($"{ctx.RequestServices.GetRequiredService<ApplicationOptions>().Name} is up and running..."));
        });
        appBuilder.UseMiddleware<AsyncRoutesMiddleware>();
        appBuilder.UseRabbitMq();
        appBuilder.UseShellBuilder();
        appBuilder.UseOcelot().Wait();
      })
      .Build().RunAsync();
  }
}
