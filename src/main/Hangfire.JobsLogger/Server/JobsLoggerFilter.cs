using Hangfire.Server;
using Hangfire.States;
using Hangfire.JobsLogger;
using System;
using System.Collections.Generic;
using System.Text;
using Hangfire.JobsLogger.Helper;

namespace Hangfire.JobsLogger.Server
{
    internal class JobsLoggerFilter : IServerFilter
    {
        private readonly JobsLoggerOptions _options;

        public JobsLoggerFilter(JobsLoggerOptions options) 
        {
            _options = options ?? new JobsLoggerOptions();
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var loggerContext = new LoggerContext();
            string item = Util.GetLoggerContextName(filterContext.BackgroundJob.Id);

            filterContext.Items[item] = loggerContext;

            loggerContext.FromPerformContext(filterContext, _options);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            string item = Util.GetLoggerContextName(filterContext.BackgroundJob.Id);
            filterContext.Items.Remove(item);
        }
    }
}
