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

        public bool IsEnabled()
        {
            return _options.LogLevel != LogLevel.None;
        }

        private int GetCounterValue(IStorageConnection connection, string jobId) 
        {
            string counterName = Utils.GetCounterName(jobId);
            var counterHash = connection.GetAllEntriesFromHash(counterName);
            int counterValue = counterHash != null && counterHash.Any() ?
                int.Parse(counterHash.FirstOrDefault().Value) : 0;

            return counterValue;
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
                int counterValue = GetCounterValue(connection, jobId);

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

        public IEnumerable<LogMessage> GetLogMessagesByJobId(IStorageConnection connection, string jobId, int from = 1, int count = int.MaxValue)
        {
            var logMessages = new List<LogMessage>();

            try
            {
                int fromValue = GetCounterValue(connection, jobId);
                int toValue = count > fromValue ? count : fromValue;

                foreach (int i in Enumerable.Range(fromValue, toValue)) 
                {
                    var logMessageHash = connection.GetAllEntriesFromHash(Utils.GetKeyName(i, jobId));

                    if (logMessageHash != null && logMessageHash.Any())
                    {
                        var logMessage = SerializationHelper
                            .Deserialize<LogMessage>(logMessageHash.FirstOrDefault().Value);
                        logMessages.Add(logMessage);
                    }
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
