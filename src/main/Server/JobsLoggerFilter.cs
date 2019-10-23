using Hangfire.Server;
using Hangfire.States;
using Hangfire.JobsLogger;
using System;
using System.Collections.Generic;
using System.Text;

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

            filterContext.Items[Common.LoggerContextName] = loggerContext;

            LoggerContext.FromPerformContext(filterContext, _options);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            filterContext.Items.Remove(Common.LoggerContextName);
        }
    }
}
