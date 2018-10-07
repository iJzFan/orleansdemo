using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;

namespace webapi.Services
{
    public class SiloService : IHostedService
    {
        private readonly ISiloHost _silo;
        private readonly ILogger<SiloService> _logger;
        public readonly IClusterClient Client;

        public SiloService(ILogger<SiloService> logger,IServiceProvider provider)
        {
            _logger = logger;
            _silo =new SiloHostBuilder().UseLocalhostClustering()
                //.ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .UseDashboard(x => x.HostSelf=false)
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(Grains.IUserGrain).Assembly).WithReferences()).EnableDirectClient()
                .Build();
            //if (!_silo.Services.GetRequiredService<IClusterClient>().IsInitialized)
            //{
            //    _silo.Services.GetRequiredService<IClusterClient>().Connect().GetAwaiter().GetResult();
            //}
            Client =_silo.Services.GetRequiredService<IClusterClient>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _silo.StartAsync(cancellationToken);
            _logger.LogInformation("Silo started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _silo.StopAsync(cancellationToken);
            _logger.LogInformation("Silo stopped");
        }
    }
}