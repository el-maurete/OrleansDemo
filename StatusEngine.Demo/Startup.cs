using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using OrleansDashboard;
using StatusEngine.Demo.Services;

namespace StatusEngine.Demo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions
                    .Converters.Add(new JsonStringEnumConverter()));
            
            services.AddSignalR()
                .AddJsonProtocol(o => o.PayloadSerializerOptions
                    .Converters.Add(new JsonStringEnumConverter()));
            
            services.AddLogging(l => l.AddConsole()
                .AddFilter("Orleans", LogLevel.Error));
            
            services.AddCors(options => options
                .AddPolicy("Default", builder => builder
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));
            
            services.AddTransient<Orleans.EventSourcing.LogStorage.LogConsistencyProvider>();
            services.AddSingleton<IStatusEngine, Services.StatusEngine>();

            services.AddHostedService<DemoClient>();

            services.AddDashboard(options => options.BasePath = "/dashboard");
            services.AddSpaStaticFiles(options => options.RootPath = "UI/dist/UI");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseOrleansDashboard(new DashboardOptions { BasePath = "/dashboard" });
            app.UseRouting();
            app.UseCors("Default");
            app.UseSpa(_ => { });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<UpdatesHub>("/updatesHub");
            });
        }
    }
}
