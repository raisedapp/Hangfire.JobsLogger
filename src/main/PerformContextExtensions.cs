using Hangfire.Common;
using Hangfire.JobsLogger.Model;
using Hangfire.JobsLogger.Server;
using Hangfire.Server;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger
{
    public static class PerformContextExtensions
    {
        private static readonly object _lock = new object();

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
            if (context.Items[Common.LoggerContextName] is LoggerContext loggerContext && loggerContext.IsEnabled())
            {
                lock (_lock)
                {
                    using (var connection = context.Storage.GetConnection())
                    using (var writeTransaction = connection.CreateWriteTransaction())
                    {
                        var logData = new LogMessage()
                        {
                            JobId = context.BackgroundJob.Id,
                            LogLevel = logLevel,
                            DateCreation = DateTime.UtcNow,
                            Message = logMessage
                        };

                        var key = Guid.NewGuid().ToString();

                        var values = new Dictionary<string, string>
                        {
                            [key] = SerializationHelper.Serialize(logData)
                        };

                        writeTransaction.SetRangeInHash(key, values);

                        if (writeTransaction is JobStorageTransaction jsTransaction)
                        {
                            jsTransaction.ExpireHash(key, LoggerContext.Options.ExpireIn);
                        }
                    }
                }
            }
        }
    }
}
