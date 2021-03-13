using Microservices.Common.Shell;
using Microservices.Common.CQRS.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.CQRS
{
  public static class Extensions
  {
    public static IShellBuilder AddCommandHandlers(this IShellBuilder shellBuilder)
    {
      shellBuilder.Services.Scan(s =>
              s.FromApplicationDependencies()
             //s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
             .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
             .AsImplementedInterfaces()
             .WithTransientLifetime());
      return shellBuilder;
    }
    public static IShellBuilder AddInMemoryCommandDispatcher(this IShellBuilder shellBuilder)
    {
      shellBuilder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
      return shellBuilder;
    }

    public static IShellBuilder AddQueryHandlers(this IShellBuilder shellBuilder)
    {
      shellBuilder.Services.Scan(s =>
                s.FromApplicationDependencies()
               //s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
               .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
               .AsImplementedInterfaces()
               .WithTransientLifetime());
      return shellBuilder;
    }

    public static IShellBuilder AddInMemoryQueryDispatcher(this IShellBuilder shellBuilder)
    {
      shellBuilder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
      return shellBuilder;
    }

    public static IShellBuilder AddEventHandlers(this IShellBuilder shellBuilder)
    {
      shellBuilder.Services.Scan(s =>
            s.FromApplicationDependencies()
               //s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
      return shellBuilder;
    }

    public static IShellBuilder AddInMemoryEventDispatcher(this IShellBuilder shellBuilder)
    {
      shellBuilder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
      return shellBuilder;
    }
  }
}