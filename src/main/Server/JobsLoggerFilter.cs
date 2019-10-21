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
            _options = options;
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            var state = filterContext.Connection.GetStateData(filterContext.BackgroundJob.Id);

            //if (state == null) return;

            //if (!string.Equals(state.Name, ProcessingState.StateName,
            //    StringComparison.OrdinalIgnoreCase)) return;

            var loggerContext = new LoggerContext();

            filterContext.Items[Common.LoggerContextName] = loggerContext;

            LoggerContext.FromPerformContext(filterContext, _options);
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            filterContext.Items.Remove(Common.LoggerContextName);
        }
    }
}
