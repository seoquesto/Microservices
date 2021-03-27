using Microsoft.AspNetCore.Http;

namespace Microservices.Api.Correlation
{
      public interface ICorrelationContextBuilder
    {
        CorrelationContext Build(HttpContext context, string correlationId, string spanContext, string name = null,
            string resourceId = null);
    }
}