using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Api.Builders;
using Microservices.Api.Correlation;
using Microservices.Api.Options;
using Microservices.Common.RabbitMq;
using Microservices.Common.RabbitMq.Conventions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Microservices.Api.Middleware
{
  public class AsyncRoutesMiddleware : IMiddleware
  {
    private static readonly ConcurrentDictionary<string, IConvention> Conventions =
        new ConcurrentDictionary<string, IConvention>();

    private readonly IBusClient _busClient;
    private readonly IDictionary<string, AsyncRouteOptions> _routes;
    private readonly IPayloadBuilder _payloadBuilder;
    private readonly ICorrelationContextBuilder _correlationContextBuilder;

    public AsyncRoutesMiddleware(AsyncRoutesOptions asyncRoutesOptions, IBusClient busClient, IPayloadBuilder payloadBuilder, ICorrelationContextBuilder correlationContextBuilder)
    {
      this._routes = asyncRoutesOptions.Routes;
      this._busClient = busClient;
      this._payloadBuilder = payloadBuilder;
      this._correlationContextBuilder = correlationContextBuilder;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      if (_routes is null || !_routes.Any())
      {
        await next(context);
        return;
      }

      var key = GetKey(context);
      if (!_routes.TryGetValue(key, out var route))
      {
        await next(context);
        return;
      }

      if (!Conventions.TryGetValue(key, out var conventions))
      {
        conventions = new MessageConvention(typeof(object), route.RoutingKey, route.Exchange, null);
        Conventions.TryAdd(key, conventions);
      }
      var spanContext = string.Empty;
      // var spanContext = _tracer.ActiveSpan is null ? string.Empty : _tracer.ActiveSpan.Context.ToString();
      var message = await _payloadBuilder.BuildFromJsonAsync<object>(context.Request);
      var resourceId = Guid.NewGuid().ToString("N");

      var messageId = Guid.NewGuid().ToString("N");
      var correlationId = Guid.NewGuid().ToString("N");
      var correlationContext = _correlationContextBuilder.Build(context, correlationId, spanContext,
          route.RoutingKey, resourceId);
      this._busClient.Send(message, conventions, messageId, correlationId, spanContext, correlationContext);
      context.Response.StatusCode = 202;
      context.Response.Headers.Add("X-Operation", $"operations/{correlationId}");
    }

    private static string GetKey(HttpContext context) => $"{context.Request.Method} {context.Request.Path}";
  }
}