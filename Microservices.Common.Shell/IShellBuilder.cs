using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Shell
{
  public interface IShellBuilder
  {
    IServiceCollection Services { get; }
    IServiceProvider Build();
    void AddBuildAction(Action<IServiceProvider> execute);
    void AddInitializer(IInitializer initializer);
    void AddInitializer<TInitializer>() where TInitializer : IInitializer;
  }
}