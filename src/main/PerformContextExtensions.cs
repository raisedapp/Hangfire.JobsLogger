using Hangfire.Common;
using Hangfire.JobsLogger.Model;
using Hangfire.JobsLogger.Server;
using Hangfire.Server;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
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
                lock (_lock)
                {
                    if (context.Items[Common.LoggerContextName] is LoggerContext loggerContext && 
                        loggerContext.IsEnabled())
                    {
                        using (var connection = context.Storage.GetConnection())
                        using (var writeTransaction = connection.CreateWriteTransaction())
                        {
                            var jobId = context.BackgroundJob.Id;

                            var logMessageModel = new LogMessage
                            {
                                JobId = jobId,
                                LogLevel = logLevel,
                                DateCreation = DateTime.UtcNow,
                                Message = logMessage
                            };

                            var logData = new List<LogMessage>
                            {
                                logMessageModel
                            };

                            var key = Utils.GetKeyName(jobId);
                            var oldValues = connection.GetAllEntriesFromHash(key);
                            var logSerialization = SerializationHelper.Serialize(logData);

                            string value = string.Empty;

                            if (oldValues != null && oldValues.Any())
                            {
                                var logArray = JArray.Parse(oldValues.FirstOrDefault().Value);
                                logArray.Add(JObject.Parse(SerializationHelper.Serialize(logMessageModel)));

                                value = logArray.ToString(Newtonsoft.Json.Formatting.None);
                            }
                            else
                            {
                                value = logSerialization;
                            }

                            var dictionary = new Dictionary<string, string>
                            {
                                [key] = value
                            };

                            writeTransaction.SetRangeInHash(key, dictionary);

                            if (writeTransaction is JobStorageTransaction jsTransaction)
                            {
                                jsTransaction.ExpireHash(key, loggerContext.GetOptions().ExpireIn);
                            }

                            writeTransaction.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Write Log. Message = {ex.Message}, StackTrace = {ex.ToString()}");
            }
        }
    }
}
