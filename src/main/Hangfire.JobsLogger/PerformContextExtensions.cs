using Hangfire.Common;
using Hangfire.JobsLogger.Helper;
using Hangfire.JobsLogger.Model;
using Hangfire.JobsLogger.Server;
using Hangfire.Server;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Hangfire.JobsLogger
{
    public static class PerformContextExtensions
    {
        public static void LogTrace(this PerformContext context, string logMessage)
        {
            Log(context, LogLevel.Trace, logMessage);
        }

        public static void LogDebug(this PerformContext context, string logMessage)
        {
            Log(context, LogLevel.Debug, logMessage);
        }

        public static void LogInformation(this PerformContext context, string logMessage)
        {
            Log(context, LogLevel.Information, logMessage);
        }

        public static void LogWarning(this PerformContext context, string logMessage)
        {
            Log(context, LogLevel.Warning, logMessage);
        }

        public static void LogError(this PerformContext context, string logMessage)
        {
            Log(context, LogLevel.Error, logMessage);
        }

        public static void LogCritical(this PerformContext context, string logMessage)
        {
            Log(context, LogLevel.Critical, logMessage);
        }

        public static void Log(this PerformContext context, LogLevel logLevel, string logMessage)
        {
            try
            {
                var jobId = context.BackgroundJob.Id;
                var item = Util.GetLoggerContextName(jobId);

                if (context.Items[item] is LoggerContext loggerContext && 
                    loggerContext.IsEnabled(logLevel))
                {
                    using (var connection = context.Storage.GetConnection()) 
                    {
                        var jobExpirationTimeout = context.Storage.JobExpirationTimeout;

                        loggerContext.SaveLogMessage(connection, jobId, jobExpirationTimeout, logLevel, logMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error Write Log. Exception Message = {ex.Message}, StackTrace = {ex.ToString()}");
            }
        }
    }
}
