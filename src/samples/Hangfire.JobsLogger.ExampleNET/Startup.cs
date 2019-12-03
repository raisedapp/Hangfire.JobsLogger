using System;
using System.Threading.Tasks;
using Hangfire.JobsLogger.ExampleShared;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Hangfire.JobsLogger.ExampleNET.Startup))]

namespace Hangfire.JobsLogger.ExampleNET
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard(string.Empty);

            var taskExample = new TaskExample();
            RecurringJob.AddOrUpdate(() => taskExample.TaskMethod(null), Cron.Minutely);
        }
    }
}