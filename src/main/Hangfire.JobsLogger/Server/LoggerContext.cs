using Hangfire.Common;
using Hangfire.JobsLogger.Extensions;
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

                string counterName = Utils.GetCounterName(jobId);
                var counterOldValue = connection.GetAllEntriesFromHash(counterName);
                int counterValue = counterOldValue != null && counterOldValue.Any() ? 
                    int.Parse(counterOldValue.FirstOrDefault().Value) : 0;

                var keyName = Utils.GetKeyName(++counterValue, jobId);
                var logSerialization = SerializationHelper.Serialize(logMessageModel);

                var dictionaryLog = new Dictionary<string, string>
                {
                    [keyName] = logSerialization
                };

                var dictionaryLogCounter = new Dictionary<string, string>
                {
                    [counterName] = Convert.ToString(counterValue)
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
