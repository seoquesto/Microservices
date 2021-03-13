using Microservices.Common.Shell;
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

    public static IShellBuilder AddPrometheus(this IShellBuilder shellBuilder, string sectionName = SectionName)
    {
      var options = shellBuilder.GetOptions<PrometheusOptions>(sectionName);
      shellBuilder.Services.AddSingleton(options);

      if (!options.Enabled)
      {
        return shellBuilder;
      }

      shellBuilder.Services.AddHostedService<PrometheusJob>();
      shellBuilder.Services.AddSystemMetrics();
      return shellBuilder;
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