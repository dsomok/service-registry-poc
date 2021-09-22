using System.Threading.Tasks;
using Refit;

namespace Exporter.ServiceRegistry
{
    public interface IServiceRegistryApi
    {
        [Post("/api/v1/register/{name}")]
        Task RegisterAsync(string name, RegistrationRequest registrationRequest);

        [Post("/api/v1/unregister/{name}")]
        Task UnregisterAsync(string name);
    }

    public class RegistrationRequest
    {
        public string Address { get; set; }
        public string HealthCheckAddress { get; set; }
    }
}
