using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microservices.Services.Comments.Infrastructure;
using Microservices.Common.Logging;

namespace Microservices.Services.Comments
{
  public class Program
  {
    public static Task Main(string[] args)
      => WebHost.CreateDefaultBuilder(args)
          .ConfigureServices(services => services.AddControllers())
          .Configure(applicationBuilder => applicationBuilder.UseInfrastructure())
          .UseLogging()
          .Build()
          .RunAsync();
  }
}
