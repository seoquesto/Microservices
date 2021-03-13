using System;
using System.Collections.Concurrent;

namespace Microservices.Common.RabbitMq
{
  public class ConventionsProvider : IConventionsProvider
  {
    private readonly ConcurrentDictionary<Type, IConvention> _conventions = new ConcurrentDictionary<Type, IConvention>();
    private readonly IConventionsBuilder _conventionsBuilder;

    public ConventionsProvider(IConventionsBuilder conventionsBuilder)
    {
      this._conventionsBuilder = conventionsBuilder;
    }

    public IConvention Get<T>() => Get(typeof(T));

    public IConvention Get(Type type)
    {
      if (_conventions.TryGetValue(type, out var conventions))
      {
        return conventions;
      }

      return _conventions.GetOrAdd(type, new MessageConvention(type, _conventionsBuilder.GetRoutingKey(type),
                        _conventionsBuilder.GetExchange(type), _conventionsBuilder.GetQueue(type)));
    }
  }
}