namespace Microservices.Common.Shell
{
  public interface IStartupInitializer : IInitializer
  {
    void AddInitializer(IInitializer initializer);
  }
}