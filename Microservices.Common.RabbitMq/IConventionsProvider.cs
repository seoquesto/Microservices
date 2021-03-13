
using System;

namespace Microservices.Common.RabbitMq
{
  public interface IConventionsProvider
  {
    IConvention Get<T>();
    IConvention Get(Type type);
  }
}