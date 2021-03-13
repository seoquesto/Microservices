using System;

namespace Microservices.Services.Posts.Core.Exceptions
{
  public abstract class DomainException : ApplicationException
  {
    public abstract string ExceptionCode { get; }
    public DomainException() : base() { }
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
  }
}