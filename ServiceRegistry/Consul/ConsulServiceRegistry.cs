using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using ServiceRegistry.Exceptions;
using ServiceRegistry.Models.Request;
using ServiceRegistry.Models.Response;

namespace ServiceRegistry.Consul
{
    internal class ConsulServiceRegistry : IServiceRegistry
    {
        private readonly IConsulClient _consulClient;

        public ConsulServiceRegistry(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public Task RegisterAsync(string serviceName, RegistrationRequest registrationRequest, CancellationToken ct)
        {
            var serviceRegistration = new AgentServiceRegistration
            {
                Name = serviceName,
                Address = registrationRequest.Address,
                Check = new AgentServiceCheck
                {
                    Name = $"{serviceName}-service-healthcheck",
                    HTTP = registrationRequest.HealthCheckAddress,
                    Interval = TimeSpan.FromSeconds(10)
                }
            };

            return _consulClient.Agent.ServiceRegister(serviceRegistration, ct);
        }

        public Task UnregisterAsync(string serviceName, CancellationToken ct)
        {
            return _consulClient.Agent.ServiceDeregister(serviceName, ct);
        }

        public async Task<ResolveResponse> ResolveAsync(string serviceName, CancellationToken ct)
        {
            var consulResponse = await _consulClient.Catalog.Service(serviceName, ct);
            if (consulResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ServiceNotFoundException(serviceName);
            }

            var service = consulResponse.Response.FirstOrDefault();
            if (service == null)
            {
                throw new ServiceNotFoundException(serviceName);
            }

            if (!await IsServiceHealthyAsync(serviceName, ct))
            {
                throw new ServiceUnhealthyException(serviceName);
            }

            return new ResolveResponse
            {
                Name = serviceName,
                Address = service.ServiceAddress
            };
        }

        private async Task<bool> IsServiceHealthyAsync(string serviceName, CancellationToken ct)
        {
            var consulResponse = await _consulClient.Health.Service(serviceName, ct);
            if (consulResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var service = consulResponse.Response.FirstOrDefault();
            if (service == null || service.Checks.Any(hc => hc.Status != HealthStatus.Passing))
            {
                return false;
            }

            return true;
        }
    }
}
