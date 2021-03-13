using System.Threading.Tasks;

namespace Microservices.Common.Shell
{
  public interface IInitializer
  {
    Task InitializeAsync();
  }
}