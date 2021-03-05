using System;
using Microservices.Common.Api;
using Microservices.Common.CQRS.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.CQRS
{
  public static class Extensions
  {
    public static IAppBuilder AddCommandHandlers(this IAppBuilder appBuilder)
    {
      appBuilder.Services.Scan(s =>
         s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
             .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
             .AsImplementedInterfaces()
             .WithTransientLifetime());
      return appBuilder;
    }
    public static IAppBuilder AddInMemoryCommandDispatcher(this IAppBuilder appBuilder)
    {
      appBuilder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
      return appBuilder;
    }

    public static IAppBuilder AddQueryHandlers(this IAppBuilder appBuilder)
    {
      appBuilder.Services.Scan(s =>
           s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
               .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
               .AsImplementedInterfaces()
               .WithTransientLifetime());
      return appBuilder;
    }

    public static IAppBuilder AddInMemoryQueryDispatcher(this IAppBuilder appBuilder)
    {
      appBuilder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
      return appBuilder;
    }
  }
}