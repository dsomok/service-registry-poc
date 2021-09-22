using System.Threading;
using System.Threading.Tasks;
using ServiceRegistry.Models.Request;
using ServiceRegistry.Models.Response;

namespace ServiceRegistry.Consul
{
    public interface IServiceRegistry
    {
        Task RegisterAsync(string serviceName, RegistrationRequest registrationRequest, CancellationToken ct);
        Task UnregisterAsync(string serviceName, CancellationToken ct);

        Task<ResolveResponse> ResolveAsync(string serviceName, CancellationToken ct);
    }
}