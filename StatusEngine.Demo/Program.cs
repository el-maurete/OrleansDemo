using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;

namespace StatusEngine.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseOrleans(builder => builder
                    .UseLocalhostClustering()
                    .AddMemoryGrainStorageAsDefault()
                    .AddCustomStorageBasedLogConsistencyProviderAsDefault())
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseStartup<Startup>());
    }
}