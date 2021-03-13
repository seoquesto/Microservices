using System;
using System.Linq;
using Microservices.Common.Shell;
using Microservices.Common.WebApi.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.WebApi
{
  public static class Extensions
  {
    public static IShellBuilder AddWebApi(this IShellBuilder shellBuilder)
    {
      shellBuilder.Services.AddLogging()
                            .AddMvcCore();
      if (shellBuilder.Services.All(s => s.ServiceType != typeof(IExceptionToResponseMapper)))
      {
        shellBuilder.Services.AddTransient<IExceptionToResponseMapper, EmptyExceptionToResponseMapper>();
      }

      return shellBuilder;
    }

    public static IShellBuilder AddErrorHandler<T>(this IShellBuilder shellBuilder)
           where T : class, IExceptionToResponseMapper
    {
      shellBuilder.Services.AddTransient<ErrorHandlerMiddleware>();
      shellBuilder.Services.AddSingleton<IExceptionToResponseMapper, T>();
      return shellBuilder;
    }

    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<ErrorHandlerMiddleware>();

    private class EmptyExceptionToResponseMapper : IExceptionToResponseMapper
    {
      public ExceptionResponse Map(Exception exception) => null;
    }
  }
}
