using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Services.Posts.Infrastructure;
using Microservices.Common.Logging;

namespace Microservices.Services.Posts
{
  public class Program
  {
    public static Task Main(string[] args)
      => WebHost.CreateDefaultBuilder(args)
          .ConfigureServices(services => services.AddInfrastructure())
          .Configure(applicationBuilder => applicationBuilder.UseInfrastructure())
          .UseLogging()
          .Build()
          .RunAsync();
  }
}
