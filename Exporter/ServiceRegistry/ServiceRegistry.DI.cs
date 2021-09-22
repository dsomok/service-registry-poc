using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Exporter.ServiceRegistry
{
    public static class DIExtensions
    {
        public static IServiceCollection AddServiceRegistryClient(this IServiceCollection services, string serviceRegistryUrl)
        {
            return services.AddSingleton(RestService.For<IServiceRegistryApi>(serviceRegistryUrl));
        }
    }
}
