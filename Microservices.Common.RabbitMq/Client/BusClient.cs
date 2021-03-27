using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Microservices.Common.RabbitMq.Client
{
  internal class BusClient : IBusClient
  {
    private readonly object _lockObject = new();
    private readonly IConnection _connection;
    private readonly ILogger<BusClient> _logger;
    private readonly bool _loggerEnabled;
    private readonly bool _persistMessages;
    private int _channelsCount;
    private readonly bool _contextEnabled;
    private readonly ConcurrentDictionary<int, IModel> _channels = new();
    private readonly int _maxChannels;
    private readonly IRabbitMqSerializer _mqSerializer;
    private readonly IContextProvider _contextProvider;

    public BusClient(IConnection connection, RabbitMqOptions options, ILogger<BusClient> logger, IRabbitMqSerializer serializer, IContextProvider contextProvider)
    {
      this._connection = connection;
      this._logger = logger;
      this._mqSerializer = serializer;
      this._contextEnabled = options.Context?.Enabled == true;
      this._loggerEnabled = options.Logger?.Enabled ?? false;
      this._persistMessages = options?.MessagesPersisted ?? false;
      this._contextProvider = contextProvider;
      this._maxChannels = options.MaxProducerChannels <= 0 ? 1000 : options.MaxProducerChannels;
    }

    public void Send(object message, IConvention convention, string messageId = null, string correlationId = null,
        string spanContext = null, object messageContext = null, IDictionary<string, object> headers = null)
    {
      var threadId = Thread.CurrentThread.ManagedThreadId;
      if (!_channels.TryGetValue(threadId, out var channel))
      {
        lock (_lockObject)
        {
          if (_channelsCount >= _maxChannels)
          {
            throw new InvalidOperationException($"Cannot create RabbitMQ producer channel for thread: {threadId} " +
                                                $"(reached the limit of {_maxChannels} channels). " +
                                                "Modify `MaxProducerChannels` setting to allow more channels.");
          }

          channel = _connection.CreateModel();
          _channels.TryAdd(threadId, channel);
          _channelsCount++;
          if (_loggerEnabled)
          {
            _logger.LogTrace($"Created a channel for thread: {threadId}, total channels: {_channelsCount}/{_maxChannels}");
          }
        }
      }
      else
      {
        if (_loggerEnabled)
        {
          _logger.LogTrace($"Reused a channel for thread: {threadId}, total channels: {_channelsCount}/{_maxChannels}");
        }
      }


      var payload = _mqSerializer.Serialize(message);
      var body = Encoding.UTF8.GetBytes(payload);
      var properties = channel.CreateBasicProperties();
      properties.Persistent = _persistMessages;
      properties.MessageId = string.IsNullOrWhiteSpace(messageId)
          ? Guid.NewGuid().ToString("N")
          : messageId;
      properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
      properties.Headers = new Dictionary<string, object>();
      properties.CorrelationId = string.IsNullOrWhiteSpace(correlationId)
             ? Guid.NewGuid().ToString("N")
             : correlationId;

      if (_contextEnabled)
      {
        IncludeMessageContext(messageContext, properties);
      }

      if (headers is { })
      {
        foreach (var (key, value) in headers)
        {
          if (string.IsNullOrWhiteSpace(key) || value is null)
          {
            continue;
          }

          properties.Headers.TryAdd(key, value);
        }
      }

      if (_loggerEnabled)
      {
        _logger.LogTrace($"Publishing a message with routing key: '{convention.RoutingKey}' " +
                         $"to exchange: '{convention.Exchange}' " +
                         $"[id: '{properties.MessageId}', correlation id: '{properties.CorrelationId}']");
      }

      channel.BasicPublish(convention.Exchange, convention.RoutingKey, properties, body);
    }

    private void IncludeMessageContext(object context, IBasicProperties properties)
    {
      if (context is { })
      {
        properties.Headers.Add(_contextProvider.HeaderName, this._mqSerializer.Serialize(context));
        return;
      }

      properties.Headers.Add(_contextProvider.HeaderName, "{}");
    }
  }
}