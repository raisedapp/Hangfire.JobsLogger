using Hangfire.Common;
using Hangfire.JobsLogger.Model;
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

namespace Hangfire.JobsLogger.Server
{
    internal class LoggerContext
    {
        private static PerformContext _context;
        private static JobsLoggerOptions _options;

        public static LoggerContext FromPerformContext(PerformContext context, 
            JobsLoggerOptions options)
        {
            _context = context;
            _options = options;

            return _context?.Items[Common.LoggerContextName] as LoggerContext ?? null;
        }

        public JobsLoggerOptions GetOptions() 
        {
            return _options;
        }

        public void SaveLogMessage(IStorageConnection connection, string jobId, TimeSpan jobExpirationTimeout, LogLevel logLevel, string logMessage) 
        {
            using (var writeTransaction = connection.CreateWriteTransaction())
            {
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

                string counterName = Utils.GetCounterName(jobId);
                var counterOldValue = connection.GetAllEntriesFromHash(counterName);
                int counterValue = counterOldValue != null && counterOldValue.Any() ? int.Parse(counterOldValue.FirstOrDefault().Value) : 0;

                int currentPage = counterValue;//TODO

                var keyName = Utils.GetKeyName(currentPage, jobId);
                var oldValues = connection.GetAllEntriesFromHash(keyName);
                var logSerialization = SerializationHelper.Serialize(logData);

                string value = string.Empty;

                if (oldValues != null && oldValues.Any())
                {
                    var logArray = JArray.Parse(oldValues.FirstOrDefault().Value);
                    logArray.Add(JObject.Parse(SerializationHelper.Serialize(logMessageModel)));

                    value = logArray.ToString(Formatting.None);
                }
                else
                {
                    value = logSerialization;
                }

                var dictionaryLog = new Dictionary<string, string>
                {
                    [keyName] = value
                };

                var dictionaryLogCounter = new Dictionary<string, string>
                {
                    [counterName] = Convert.ToString(++counterValue)
                };

                writeTransaction.SetRangeInHash(keyName, dictionaryLog);
                writeTransaction.SetRangeInHash(counterName, dictionaryLogCounter);

                if (writeTransaction is JobStorageTransaction jsTransaction)
                {
                    jsTransaction.ExpireHash(keyName, jobExpirationTimeout);
                    jsTransaction.ExpireHash(counterName, jobExpirationTimeout);
                }

                writeTransaction.Commit();
            }
        }

        public bool IsEnabled()
        {
            return _options.LogLevel != LogLevel.None;
        }
    }
}
