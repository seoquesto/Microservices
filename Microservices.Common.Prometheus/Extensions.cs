using Microservices.Common.Api;
using Microservices.Common.Prometheus.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Prometheus.SystemMetrics;

namespace Microservices.Common.Prometheus
{
  public static class Extensions
  {
    private const string SectionName = "prometheus";

    public static IAppBuilder AddPrometheus(this IAppBuilder appBuilder, string sectionName = SectionName)
    {
      var options = appBuilder.GetOptions<PrometheusOptions>(sectionName);

      if (!options.Enabled)
      {
        return appBuilder;
      }

      appBuilder.Services.AddSingleton(options);
      appBuilder.Services.AddHostedService<PrometheusJob>();
      appBuilder.Services.AddSystemMetrics();
      return appBuilder;
    }

    public static IApplicationBuilder UsePrometheus(this IApplicationBuilder app)
    {
      var options = app.ApplicationServices.GetRequiredService<PrometheusOptions>();
      if (!options.Enabled)
      {
        return app;
      }

      var endpoint = string.IsNullOrWhiteSpace(options.Endpoint) ? "/metrics" :
          options.Endpoint.StartsWith("/") ? options.Endpoint : $"/{options.Endpoint}";

      return app
          .UseHttpMetrics()
          .UseGrpcMetrics()
          .UseMetricServer(endpoint);
    }
  }
}