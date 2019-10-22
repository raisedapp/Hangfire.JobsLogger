using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.LiteDB;
using Hangfire.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hangfire.JobsLogger.ExampleCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseJobsLogger()
                .UseLiteDbStorage());

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            RecurringJob.AddOrUpdate(() => TaskMethod(null), Cron.Minutely);
        }

        public void TaskMethod(PerformContext context)
        {
            context.LogTrace($"Trace Message.. {DateTime.UtcNow.Ticks}");
            context.LogDebug($"Debug Message.. {DateTime.UtcNow.Ticks}");
            context.LogInformation($"Information Message.. {DateTime.UtcNow.Ticks}");
            context.LogWarning($"Warning Message.. {DateTime.UtcNow.Ticks}");
            context.LogError($"Error Message.. {DateTime.UtcNow.Ticks}");
            context.LogCritical($"Critical Message.. {DateTime.UtcNow.Ticks}");
        }
    }
}
