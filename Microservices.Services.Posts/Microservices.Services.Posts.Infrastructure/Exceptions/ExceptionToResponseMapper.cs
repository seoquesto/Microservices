using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using Microservices.Common.WebApi;
using Microservices.Services.Posts.Core.Exceptions;

namespace Microservices.Services.Posts.Infrastructure.Exceptions
{
  internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
  {
    private static readonly ConcurrentDictionary<Type, string> Codes = new ConcurrentDictionary<Type, string>();

    public ExceptionResponse Map(Exception exception)
        => exception switch
        {
          DomainException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                  HttpStatusCode.BadRequest),
          _ => new ExceptionResponse(new { code = "error", reason = "There was an error." },
                  HttpStatusCode.BadRequest)
        };

    private static string GetCode(Exception exception)
    {
      var type = exception.GetType();
      if (Codes.TryGetValue(type, out var code))
      {
        return code;
      }

      var exceptionCode = exception switch
      {
        DomainException domainException when !string.IsNullOrWhiteSpace(domainException.ExceptionCode) => domainException
            .ExceptionCode,
        _ => ToUnderscoreCase(exception.GetType().Name).Replace("_exception", string.Empty)
      };

      Codes.TryAdd(type, exceptionCode);

      return exceptionCode;
    }

    private static string ToUnderscoreCase(string str)
       => string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
           .ToLowerInvariant();
  }
}