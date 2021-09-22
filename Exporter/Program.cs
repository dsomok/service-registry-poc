using System.Threading.Tasks;
using Exporter.ServiceRegistry;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Exporter
{
    public class Program
    {
        private const string ServiceName = "exporter";

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();
            var serviceRegistryApi = host.Services.GetRequiredService<IServiceRegistryApi>();
            var exporterUrl = configuration["Exporter"];
            await serviceRegistryApi.RegisterAsync(ServiceName, new RegistrationRequest
            {
                Address = exporterUrl,
                HealthCheckAddress = $"{exporterUrl}/hc"
            });

            await host.RunAsync();

            await serviceRegistryApi.UnregisterAsync(ServiceName);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
