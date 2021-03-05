namespace Microservices.Common.Types
{
  public interface IIdentifiable<T>
  {
    T Id { get; }
  }
}
