using System.Collections.Generic;

namespace Microservices.Common.Logging.Options
{
  public class LoggingOptions
  {
    public string Level { get; set; }
    public FileOptions File { get; set; }
    public ConsoleOptions Console { get; set; }
    public SeqOptions Seq { get; set; }
    public IEnumerable<string> ExcludePaths { get; set; }
  }

  public class FileOptions
  {
    public bool Enabled { get; set; }
    public string Path { get; set; }
    public string Interval { get; set; }
  }

  public class ConsoleOptions
  {
    public bool Enabled { get; set; }
  }

  public class SeqOptions
  {
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public string ApiKey { get; set; }
  }
}