using System;
using System.Threading.Tasks;

namespace Microservices.Common.RabbitMq
{
  public interface IBusSubscriber
  {
    IBusSubscriber Subscribe<T>(Func<IServiceProvider, T, Task> handle) where T : class;
  }
}