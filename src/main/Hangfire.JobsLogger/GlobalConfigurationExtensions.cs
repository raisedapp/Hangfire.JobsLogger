using Hangfire.Dashboard;
using Hangfire.JobsLogger.Dashboard;
using Hangfire.JobsLogger.Server;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;
using Hangfire.JobsLogger.Dashboard.Pages.Html;

namespace Hangfire.JobsLogger
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration UseJobsLogger(this IGlobalConfiguration configuration,
            JobsLoggerOptions options = null)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            options = options ?? new JobsLoggerOptions();

            GlobalJobFilters.Filters.Add(new JobsLoggerFilter(options));

            DashboardRoutes.Routes.AddRazorPage(Dashboard.Pages.LoggerPage.PageRoute,
                x => new Dashboard.Pages.Html.Logging());
            JobHistoryRenderer.Register(EnqueuedState.StateName, new EnqueuedStateRenderer().Render);

            return configuration;
        }
    }
}
