using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Shell
{
  public static class Extensions
  {
    private const string SectionName = "application";

    public static IShellBuilder AddShellBuilder(this IServiceCollection serviceCollection, string sectionName = SectionName)
    {
      var builder = ShellBuilder.Create(serviceCollection);
      var appOptions = builder.GetOptions<ApplicationOptions>(sectionName);
      builder.Services.AddSingleton(appOptions);

      Console.WriteLine(Figgle.FiggleFonts.Doom.Render($"{appOptions.Name}{appOptions.Version}"));

      return builder;
    }

    public static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName)
     where TModel : new()
    {
      var model = new TModel();
      configuration.GetSection(sectionName).Bind(model);
      return model;
    }

    public static TModel GetOptions<TModel>(this IShellBuilder builder, string settingsSectionName)
        where TModel : new()
    {
      using var serviceProvider = builder.Services.BuildServiceProvider();
      var configuration = serviceProvider.GetService<IConfiguration>();
      return configuration.GetOptions<TModel>(settingsSectionName);
    }

    public static IApplicationBuilder UseShellBuilder(this IApplicationBuilder app)
    {
      using (var scope = app.ApplicationServices.CreateScope())
      {
        var initializer = scope.ServiceProvider.GetRequiredService<IStartupInitializer>();
        Task.Run(() => initializer.InitializeAsync()).GetAwaiter().GetResult();
      }

      return app;
    }
  }
}