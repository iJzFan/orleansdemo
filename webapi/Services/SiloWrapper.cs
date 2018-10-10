using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Services
{
	public class SiloWrapper : IHostedService
	{
		private readonly ISiloHost _silo;
		public readonly IClusterClient Client;

		public SiloWrapper()
		{
			_silo = new SiloHostBuilder().UseLocalhostClustering()
				.ConfigureApplicationParts(parts =>
					parts.AddApplicationPart(typeof(Grains.IUserGrain).Assembly).WithReferences()).EnableDirectClient()
					.AddMongoDBGrainStorageAsDefault(options =>
					{
						options.ConnectionString = "mongodb://mongo01/OrleansTestApp";
					})
					.ConfigureLogging(x =>
					{
						x.AddConsole();
						x.SetMinimumLevel(LogLevel.Warning);
					})
					.UseDashboard(x =>
					{
						x.HostSelf = false;
					})
					.UseTransactions().Build();

			Client = _silo.Services.GetRequiredService<IClusterClient>();
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _silo.StartAsync(cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _silo.StopAsync(cancellationToken);
		}
	}
}