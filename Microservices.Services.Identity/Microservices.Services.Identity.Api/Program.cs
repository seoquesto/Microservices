using System.Linq;
using System.Threading.Tasks;
using Microservices.Common.AppMetrics;
using Microservices.Common.Logging;
using Microservices.Common.Prometheus;
using Microservices.Common.Shell;
using Microservices.Common.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microservices.Services.Identity.Api
{
  public class Program
  {
    public static Task Main(string[] args)
    => WebHost.CreateDefaultBuilder(args)
        .ConfigureServices(serviceCollection =>
        {

          serviceCollection.AddControllers();
            // serviceCollection
            //   .AddHostedService<MetricsJob>();

            serviceCollection
              .AddShellBuilder()
              .AddWebApi()
              .AddPrometheus()
              .AddMetrics()
              .Build();

        })
        .Configure(applicationBuilder =>
        {
          applicationBuilder
            .UseRouting()
            .UseEndpoints(endpoints => endpoints.MapControllers())
            .UsePrometheus()
            .UseMetrics()
            .UseShellBuilder();
        })
        .UseLogging()
        .Build()
        .RunAsync();
  }
}
