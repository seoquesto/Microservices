namespace Microservices.Common.MongoDb
{
  public interface IIdentifiable<T>
  {
    T Id { get; }
  }
}
