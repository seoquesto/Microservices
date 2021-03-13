using System;

namespace Microservices.Common.WebApi
{
  public interface IExceptionToResponseMapper
  {
    ExceptionResponse Map(Exception exception);
  }
}