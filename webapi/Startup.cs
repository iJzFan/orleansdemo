using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using webapi.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SiloService>();
            services.AddSingleton<IHostedService>(x=>x.GetRequiredService<SiloService>());
//            services.AddSingleton<IClusterClient>(x=>
//            {
//                var client = new ClientBuilder().UseLocalhostClustering()
//                    .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
//                    .ConfigureApplicationParts(parts =>
//                        parts.AddApplicationPart(typeof(Grains.IUserGrain).Assembly).WithReferences()).Build();
//                client.Connect().GetAwaiter().GetResult();
//                return client;
//            });
            //services.AddSingleton(c => c.GetRequiredService<IClusterClient>());
            services.AddServicesForSelfHostedDashboard();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IClusterClient>(x => x.GetRequiredService<SiloService>().Client);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
        }
    }
}