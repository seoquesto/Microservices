using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Api
{
  public static class Extensions
  {
    private const string SectionName = "application";
    
    public static IAppBuilder AddAppBuilder(this IServiceCollection serviceCollection, string sectionName = SectionName)
    {
      var builder = AppBuilder.Create(serviceCollection);
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

    public static TModel GetOptions<TModel>(this IAppBuilder builder, string settingsSectionName)
        where TModel : new()
    {
      using var serviceProvider = builder.Services.BuildServiceProvider();
      var configuration = serviceProvider.GetService<IConfiguration>();
      return configuration.GetOptions<TModel>(settingsSectionName);
    }
  }
}