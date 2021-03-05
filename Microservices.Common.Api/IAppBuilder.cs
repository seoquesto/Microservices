using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Api
{
  public interface IAppBuilder
  {
    IServiceCollection Services { get; }
    IServiceProvider Build();
  }
}