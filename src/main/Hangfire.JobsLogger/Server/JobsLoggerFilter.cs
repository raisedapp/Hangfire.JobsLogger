using Hangfire.Server;
using Hangfire.States;
using Hangfire.JobsLogger;
using System;
using System.Collections.Generic;
using System.Text;
using Hangfire.JobsLogger.Helper;
using System.Collections.Concurrent;

namespace Hangfire.JobsLogger.Server
{
    internal class JobsLoggerFilter : IServerFilter
    {
        public static ConcurrentDictionary<string, LoggerContext> Loggers { get; private set; } 
            = new ConcurrentDictionary<string, LoggerContext>();
        public static JobsLoggerOptions Options { get; private set; }

        public JobsLoggerFilter(JobsLoggerOptions options) 
        {
            Options = options ?? new JobsLoggerOptions();
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var loggerContext = new LoggerContext();
            string item = Util.GetLoggerContextName(filterContext.BackgroundJob.Id);

            Loggers.TryAdd(item, loggerContext);
            loggerContext.SetPerformContext(filterContext, Options);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            string item = Util.GetLoggerContextName(filterContext.BackgroundJob.Id);
            
            Loggers.TryRemove(item, out _);
        }
    }
}
