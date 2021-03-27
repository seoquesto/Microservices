using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Prometheus.DotNetRuntime;

namespace Microservices.Common.Prometheus.Internal
{
  internal sealed class PrometheusJob : IHostedService
  {
    private IDisposable _collector;
    private readonly bool _enabled;

    public PrometheusJob(PrometheusOptions options)
    {
      _enabled = options.Enabled;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      if (_enabled)
      {
        _collector = DotNetRuntimeStatsBuilder
            .Customize()
            .WithContentionStats()
            .WithJitStats()
            .WithThreadPoolSchedulingStats()
            .WithThreadPoolStats()
            .WithGcStats()
            .WithExceptionStats()
            .StartCollecting();
      }

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _collector?.Dispose();

      return Task.CompletedTask;
    }
  }
}