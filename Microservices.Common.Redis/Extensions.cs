using Microservices.Common.Shell;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Microservices.Common.Redis
{
  public static class Extensions
  {
    private const string SectionName = "redis";

    public static IShellBuilder AddRedis(this IShellBuilder shellBuilder, string sectionName = SectionName)
    {
      var options = shellBuilder.GetOptions<RedisOptions>(sectionName);

      shellBuilder.Services
          .AddSingleton(options)
          .AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(options.ConnectionString))
          .AddTransient(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(options.Database))
          .AddStackExchangeRedisCache(o =>
          {
            o.Configuration = options.ConnectionString;
            o.InstanceName = options.Instance;
          });

      return shellBuilder;
    }
  }
}