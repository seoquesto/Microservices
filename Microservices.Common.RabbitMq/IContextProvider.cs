using System.Collections.Generic;

namespace Microservices.Common.RabbitMq
{
  public interface IContextProvider
  {
    string HeaderName { get; }
    object Get(IDictionary<string, object> headers);
  }
}