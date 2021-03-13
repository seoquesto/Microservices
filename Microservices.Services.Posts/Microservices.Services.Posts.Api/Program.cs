using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microservices.Services.Posts.Infrastructure;

namespace Microservices.Services.Posts
{
  public class Program
  {
    public static Task Main(string[] args)
      => WebHost.CreateDefaultBuilder(args)
          .ConfigureServices(services =>
          {
            services.AddControllers();
            services.AddInfrastructure();
          })
          .Configure(applicationBuilder =>
          {
            applicationBuilder.UseInfrastructure();
            applicationBuilder.UseRouting();
            applicationBuilder.UseEndpoints(endpoints =>
            {
              endpoints.MapControllers();
            });
          }).Build().RunAsync();
  }
}
