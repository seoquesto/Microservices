using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Shell
{
  public class ShellBuilder : IShellBuilder
  {
    private readonly IServiceCollection _serviceCollection;
    private readonly List<Action<IServiceProvider>> _buildActions;
    public IServiceCollection Services => _serviceCollection;

    private ShellBuilder(IServiceCollection serviceCollection)
    {
      this._serviceCollection = serviceCollection;
      this._buildActions = new List<Action<IServiceProvider>>();
      this._serviceCollection.AddSingleton<IStartupInitializer>(new StartupInitializer());
    }

    public static IShellBuilder Create(IServiceCollection serviceCollection) => new ShellBuilder(serviceCollection);

    public void AddBuildAction(Action<IServiceProvider> execute) => _buildActions.Add(execute);

    public void AddInitializer(IInitializer initializer)
      => AddBuildAction(sp =>
      {
        var startupInitializer = sp.GetService<IStartupInitializer>();
        startupInitializer.AddInitializer(initializer);
      });

    public void AddInitializer<TInitializer>() where TInitializer : IInitializer
      => AddBuildAction(sp =>
      {
        var initializer = sp.GetService<TInitializer>();
        var startupInitializer = sp.GetService<IStartupInitializer>();
        startupInitializer.AddInitializer(initializer);
      });

    public IServiceProvider Build()
    {
      var serviceProvider = this._serviceCollection.BuildServiceProvider();
      _buildActions.ForEach(a => a(serviceProvider));
      return serviceProvider;
    }
  }
}