using Hangfire.JobsLogger.ExampleShared;
using Hangfire.LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                .UseLiteDbStorage(TaskExample.ConnectiongStringLiteDb));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ApplicationLogging.LoggerFactory = loggerFactory;

            app.UseHangfireServer();
            app.UseHangfireDashboard(string.Empty);

            var taskExample = new TaskExample();
            RecurringJob.AddOrUpdate(() => taskExample.TaskMethod(null), Cron.Minutely);
        }
    }
}
