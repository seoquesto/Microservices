using System;
using Microservices.Common.Logging.Options;
using Microservices.Common.Shell;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using System.Linq;

namespace Microservices.Common.Logging
{
  public static class Extensions
  {
    private const string LoggingSection = "logging";
    private const string ApplicationSection = "application";
    internal static LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch();

    public static IWebHostBuilder UseLogging(
      this IWebHostBuilder webHostBuilder,
      string loggingSection = LoggingSection,
      string applicationSection = ApplicationSection)
    {
      webHostBuilder.UseSerilog((context, configuration) =>
      {
        var loggerOptions = context.Configuration.GetOptions<LoggingOptions>(loggingSection);
        var appOptions = context.Configuration.GetOptions<ApplicationOptions>(applicationSection);

        LoggingLevelSwitch.MinimumLevel = Enum.TryParse<LogEventLevel>(loggerOptions.Level, true, out var logLevel)
              ? logLevel
              : LogEventLevel.Information;


        configuration.Enrich.FromLogContext()
                .MinimumLevel.ControlledBy(LoggingLevelSwitch)
                .Enrich.WithProperty("Service", appOptions.Service)
                .Enrich.WithProperty("Version", appOptions.Version)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);

        loggerOptions.ExcludePaths?.ToList().ForEach(p => configuration.Filter
                 .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

        var consoleOptions = loggerOptions.Console ?? new ConsoleOptions();
        var fileOptions = loggerOptions.File ?? new FileOptions();
        var seqOptions = loggerOptions.Seq ?? new SeqOptions();

        if (fileOptions.Enabled)
        {
          var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "logs/logs.txt" : fileOptions.Path;
          if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
          {
            interval = RollingInterval.Day;
          }

          configuration.WriteTo.File(path, rollingInterval: interval);
        }
        if (consoleOptions.Enabled)
        {
          configuration.WriteTo.Console();
        }

        if (seqOptions.Enabled)
        {
          configuration.WriteTo.Seq(seqOptions.Url, apiKey: seqOptions.ApiKey);
        }
      });

      return webHostBuilder;
    }
  }
}
