using Hangfire.Common;
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
            try
            {
                if (context.Items[Common.LoggerContextName] is LoggerContext loggerContext && 
                    loggerContext.IsEnabled())
                {
                    using (var connection = context.Storage.GetConnection()) 
                    {
                        var jobExpirationTimeout = context.Storage.JobExpirationTimeout;
                        var jobId = context.BackgroundJob.Id;

                        loggerContext.SaveLogMessage(connection, jobId, jobExpirationTimeout, logLevel, logMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Write Log. Exception Message = {ex.Message}, StackTrace = {ex.ToString()}");
            }
        }

        internal static IEnumerable<LogMessage> GetLogMessagesByJobId(this PerformContext context, string jobId)
        {
            var logMessages = new List<LogMessage>();

            try
            {
                using (var connection = context.Storage.GetConnection())
                {
                    //var key = Utils.GetKeyName(string.Empty, jobId);
                    //var dictionaryMessages = connection.GetAllEntriesFromHash(key);
                    //var jsonArray = dictionaryMessages.FirstOrDefault().Value;
                    //
                    //logMessages.AddRange(JsonConvert.DeserializeObject<List<LogMessage>>(jsonArray));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Read Log Messages. Exception Message = {ex.Message}, StackTrace = {ex.ToString()}");
            }

            return logMessages;
        }
    }
}
