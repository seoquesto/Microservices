using System;
using System.Linq;
using System.Reflection;

namespace Microservices.Common.RabbitMq.Conventions
{
  internal class ConventionsBuilder : IConventionsBuilder
  {

    private readonly RabbitMqOptions _options;
    private readonly string _queueTemplate;

    public ConventionsBuilder(RabbitMqOptions options)
    {
      _options = options;
      _queueTemplate = string.IsNullOrWhiteSpace(_options.Queue?.Template)
          ? "{{assembly}}/{{exchange}}.{{message}}"
          : options.Queue.Template;
    }

    public string GetRoutingKey(Type type)
    {
      var routingKey = type.Name;
      if (_options.Conventions?.MessageAttribute?.IgnoreRoutingKey is true)
      {
        return SnakeCase(routingKey);
      }

      var attribute = GetAttribute(type);
      routingKey = string.IsNullOrWhiteSpace(attribute?.RoutingKey) ? routingKey : attribute.RoutingKey;

      return SnakeCase(routingKey);
    }

    public string GetExchange(Type type)
    {
      var exchange = string.IsNullOrWhiteSpace(_options.Exchange?.Name)
          ? type.Assembly.GetName().Name
          : _options.Exchange.Name;
      if (_options.Conventions?.MessageAttribute?.IgnoreExchange is true)
      {
        return SnakeCase(exchange); ;
      }

      var attribute = GetAttribute(type);
      exchange = string.IsNullOrWhiteSpace(attribute?.Exchange) ? exchange : attribute.Exchange;

      return SnakeCase(exchange);
    }

    public string GetQueue(Type type)
    {
      var attribute = GetAttribute(type);
      var ignoreQueue = _options.Conventions?.MessageAttribute?.IgnoreQueue;
      if ((ignoreQueue is null || ignoreQueue == false) && !string.IsNullOrWhiteSpace(attribute?.Queue))
      {
        return SnakeCase(attribute.Queue);
      }

      var ignoreExchange = _options.Conventions?.MessageAttribute?.IgnoreExchange;
      var assembly = type.Assembly.GetName().Name;
      var message = type.Name;
      var exchange = ignoreExchange is true
          ? _options.Exchange?.Name
          : string.IsNullOrWhiteSpace(attribute?.Exchange)
              ? _options.Exchange?.Name
              : attribute.Exchange;
      var queue = _queueTemplate.Replace("{{assembly}}", assembly)
          .Replace("{{exchange}}", exchange)
          .Replace("{{message}}", message);

      return SnakeCase(queue);
    }

    private static string SnakeCase(string value)
        => string.Concat(value.Select((x, i) =>
                i > 0 && value[i - 1] != '.' && value[i - 1] != '/' && char.IsUpper(x) ? "_" + x : x.ToString()))
            .ToLowerInvariant();

    private static MessageAttribute GetAttribute(MemberInfo type) => type.GetCustomAttribute<MessageAttribute>();
  }
}