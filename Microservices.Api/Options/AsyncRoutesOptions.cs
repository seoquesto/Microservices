using System.Collections.Generic;

namespace Microservices.Api.Options
{
  public class AsyncRoutesOptions
  {
    public bool? Authenticate { get; set; }
    public IDictionary<string, AsyncRouteOptions> Routes { get; set; }
  }

  public class AsyncRouteOptions
  {
    public bool? Authenticate { get; set; }
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
  }
}