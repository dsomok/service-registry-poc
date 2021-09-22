using System;
using Consul;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceRegistry.Consul
{
    public static class DIExtensions
    {
        public static IServiceCollection AddConsulRegistry(this IServiceCollection services, Uri consulUri)
        {
            services.AddSingleton<IConsulClient>(new ConsulClient(config =>
            {
                config.Address = consulUri;
            }));

            services.AddSingleton<IServiceRegistry, ConsulServiceRegistry>();

            return services;
        }
    }
}
