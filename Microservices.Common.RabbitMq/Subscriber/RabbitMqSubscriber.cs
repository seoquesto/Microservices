using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microservices.Common.RabbitMq.MessageAccessor;

namespace Microservices.Common.RabbitMq.Subscriber
{
  internal class RabbitMqSubscriber : IBusSubscriber
  {
    private readonly IConnection _connection;
    private readonly IConventionsProvider _conventionProvider;
    private readonly RabbitMqOptions _options;
    private readonly RabbitMqOptions.QosOptions _qosOptions;
    private readonly ILogger<RabbitMqSubscriber> _logger;
    private readonly IRabbitMqSerializer _rabbitMqSerializer;
    private readonly IServiceProvider _serviceProvider;
    private readonly IContextProvider _contextProvider;
    private readonly bool _loggerEnabled;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
      PropertyNameCaseInsensitive = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true
    };

    public RabbitMqSubscriber(IServiceProvider serviceProvider)
    {
      this._serviceProvider = serviceProvider;
      this._connection = this._serviceProvider.GetRequiredService<IConnection>();
      this._conventionProvider = this._serviceProvider.GetRequiredService<IConventionsProvider>();
      this._options = this._serviceProvider.GetRequiredService<RabbitMqOptions>();
      this._logger = this._serviceProvider.GetRequiredService<ILogger<RabbitMqSubscriber>>();
      this._rabbitMqSerializer = this._serviceProvider.GetRequiredService<IRabbitMqSerializer>();
      this._qosOptions = _options.Qos ?? new RabbitMqOptions.QosOptions();
      this._contextProvider = _serviceProvider.GetRequiredService<IContextProvider>();
      this._loggerEnabled = _options.Logger?.Enabled ?? false;
      if (_qosOptions.PrefetchCount < 1)
      {
        _qosOptions.PrefetchCount = 1;
      }

      if (_loggerEnabled && _options.Logger?.LogConnectionStatus is true)
      {
        _connection.CallbackException += ConnectionOnCallbackException;
        _connection.ConnectionShutdown += ConnectionOnConnectionShutdown;
        _connection.ConnectionBlocked += ConnectionOnConnectionBlocked;
        _connection.ConnectionUnblocked += ConnectionOnConnectionUnblocked;
      }
    }

    public IBusSubscriber Subscribe<T>(Func<IServiceProvider, T, Task> handle) where T : class
    {
      var conventions = this._conventionProvider.Get<T>();
      var channel = _connection.CreateModel();

      this._logger.LogTrace($"Created a channel: {channel.ChannelNumber}");
      var info = $" [queue: '{conventions.Queue}', routing key: '{conventions.RoutingKey}', " +
                            $"exchange: '{conventions.Exchange}']";

      var declare = _options.Queue?.Declare ?? true;
      var durable = _options.Queue?.Durable ?? true;
      var exclusive = _options.Queue?.Exclusive ?? false;
      var autoDelete = _options.Queue?.AutoDelete ?? false;

      channel.QueueDeclare(conventions.Queue, durable, exclusive, autoDelete, new Dictionary<string, object>());

      channel.QueueBind(conventions.Queue, conventions.Exchange, conventions.RoutingKey);
      channel.BasicQos(this._qosOptions.PrefetchSize, this._qosOptions.PrefetchCount, this._qosOptions.Global);

      var consumer = new AsyncEventingBasicConsumer(channel);

      consumer.Received += async (model, args) =>
      {
        var messageId = args.BasicProperties.MessageId;
        var correlationId = args.BasicProperties.CorrelationId;
        var timestamp = args.BasicProperties.Timestamp.UnixTime;
        _logger.LogInformation($"Received a message with id: '{messageId}', " +
                               $"correlation id: '{correlationId}', timestamp: {timestamp}{info}.");

        var payload = Encoding.UTF8.GetString(args.Body.Span);
        var message = _rabbitMqSerializer.Deserialize<T>(payload);

        using var scope = _serviceProvider.CreateScope();
        var messagePropertiesAccessor = scope.ServiceProvider.GetRequiredService<IMessagePropertiesAccessor>();
        messagePropertiesAccessor.MessageProperties = new MessageProperties
        {
          MessageId = args.BasicProperties.MessageId,
          CorrelationId = args.BasicProperties.CorrelationId,
          Timestamp = args.BasicProperties.Timestamp.UnixTime,
          Headers = args.BasicProperties.Headers
        };
        var correlationContextAccessor = scope.ServiceProvider.GetRequiredService<ICorrelationContextAccessor>();
        var correlationContext = _contextProvider.Get(args.BasicProperties.Headers);
        correlationContextAccessor.CorrelationContext = correlationContext;

        await handle(this._serviceProvider, message);
        channel.BasicAck(args.DeliveryTag, false);
      };

      channel.BasicConsume(conventions.Queue, false, consumer);

      return this;

    }

    private void ConnectionOnCallbackException(object sender, CallbackExceptionEventArgs eventArgs)
    {
      _logger.LogError("RabbitMQ callback exception occured.");
      if (eventArgs.Exception is not null)
      {
        _logger.LogError(eventArgs.Exception, eventArgs.Exception.Message);
      }

      if (eventArgs.Detail is not null)
      {
        _logger.LogError(JsonSerializer.Serialize(eventArgs.Detail, SerializerOptions));
      }
    }

    private void ConnectionOnConnectionShutdown(object sender, ShutdownEventArgs eventArgs)
    {
      _logger.LogError($"RabbitMQ connection shutdown occured. Initiator: '{eventArgs.Initiator}', " +
                       $"reply code: '{eventArgs.ReplyCode}', text: '{eventArgs.ReplyText}'.");
    }

    private void ConnectionOnConnectionBlocked(object sender, ConnectionBlockedEventArgs eventArgs)
    {
      _logger.LogError($"RabbitMQ connection has been blocked. {eventArgs.Reason}");
    }

    private void ConnectionOnConnectionUnblocked(object sender, EventArgs eventArgs)
    {
      _logger.LogInformation($"RabbitMQ connection has been unblocked.");
    }
  }
}