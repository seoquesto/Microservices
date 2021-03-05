using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Api
{
  public class AppBuilder : IAppBuilder
  {
    private readonly IServiceCollection _serviceCollection;
    public IServiceCollection Services => _serviceCollection;

    private AppBuilder(IServiceCollection serviceCollection)
    {
      this._serviceCollection = serviceCollection;
    }

    public static IAppBuilder Create(IServiceCollection serviceCollection)
    {
      return new AppBuilder(serviceCollection);
    }

    public IServiceProvider Build()
    {
      var serviceProvider = this._serviceCollection.BuildServiceProvider();
      return serviceProvider;
    }
  }
}