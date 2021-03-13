using System.Linq;
using System.Threading.Tasks;
using Microservices.Common.CQRS;
using Microservices.Common.Shell;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Microservices.Common.RabbitMq
{
  public static class Extensions
  {
    private const string SectionName = "rabbitmq";
    public static IShellBuilder AddRabbitMq(this IShellBuilder shellBuilder, string sectionName = SectionName)
    {
      var options = shellBuilder.GetOptions<RabbitMqOptions>(sectionName);
      shellBuilder.Services.AddSingleton(options);

      var connectionFactory = new ConnectionFactory
      {
        Port = options.Port,
        VirtualHost = options.VirtualHost,
        UserName = options.Username,
        Password = options.Password,
        RequestedHeartbeat = options.RequestedHeartbeat,
        RequestedConnectionTimeout = options.RequestedConnectionTimeout,
        SocketReadTimeout = options.SocketReadTimeout,
        SocketWriteTimeout = options.SocketWriteTimeout,
        RequestedChannelMax = options.RequestedChannelMax,
        RequestedFrameMax = options.RequestedFrameMax,
        UseBackgroundThreadsForIO = options.UseBackgroundThreadsForIO,
        DispatchConsumersAsync = true,
        ContinuationTimeout = options.ContinuationTimeout,
        HandshakeContinuationTimeout = options.HandshakeContinuationTimeout,
        NetworkRecoveryInterval = options.NetworkRecoveryInterval,
      };

      var connection = connectionFactory.CreateConnection(options.HostNames.ToList(), options.ConnectionName);
      shellBuilder.Services.AddSingleton(connection);
      shellBuilder.Services.AddSingleton<IConventionsBuilder, ConventionsBuilder>();
      shellBuilder.Services.AddSingleton<IConventionsProvider, ConventionsProvider>();
      shellBuilder.Services.AddSingleton<IRabbitMqSerializer, NewtonsoftJsonRabbitMqSerializer>();
      shellBuilder.Services.AddSingleton<IBusClient, BusClient>();
      shellBuilder.Services.AddSingleton<IBusPublisher, RabbitMqPublisher>();
      shellBuilder.Services.AddTransient<RabbitMqExchangeInitializer>();
      shellBuilder.Services.AddHostedService<RabbitMqHostedService>();

      shellBuilder.AddInitializer<RabbitMqExchangeInitializer>();

      shellBuilder.Services.AddSingleton<IBusSubscriber, RabbitMqSubscriber>();

      return shellBuilder;
    }

    public static Task SendAsync<TCommand>(this IBusPublisher busPublisher, TCommand command)
           where TCommand : class, ICommand
           => busPublisher.PublishAsync(command);

    public static Task PublishAsync<TEvent>(this IBusPublisher busPublisher, TEvent @event)
        where TEvent : class, IEvent
        => busPublisher.PublishAsync(@event);

    public static IBusSubscriber SubscribeCommand<T>(this IBusSubscriber busSubscriber) where T : class, ICommand
        => busSubscriber.Subscribe<T>(async (serviceProvider, command) =>
        {
          using var scope = serviceProvider.CreateScope();
          await scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>().HandleAsync(command);
        });

    public static IBusSubscriber SubscribeEvent<T>(this IBusSubscriber busSubscriber) where T : class, IEvent
        => busSubscriber.Subscribe<T>(async (serviceProvider, @event) =>
        {
          using var scope = serviceProvider.CreateScope();
          await scope.ServiceProvider.GetRequiredService<IEventHandler<T>>().HandleAsync(@event);
        });

    public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
           => new RabbitMqSubscriber(app.ApplicationServices);
  }
}
