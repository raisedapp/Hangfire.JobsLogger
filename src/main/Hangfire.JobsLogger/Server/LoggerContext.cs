using Hangfire.Common;
using Hangfire.JobsLogger.Helper;
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
        private PerformContext _context;
        private JobsLoggerOptions _options;

        private readonly object _lockObj = new object();

        public LoggerContext FromPerformContext(PerformContext context, 
            JobsLoggerOptions options)
        {
            _context = context;
            _options = options;

            return _context?.Items[Util.GetLoggerContextName(context.BackgroundJob.Id)] as LoggerContext ?? null;
        }

        public JobsLoggerOptions GetOptions() 
        {
            return _options;
        }

        public bool IsEnabled()
        {
            return _options.LogLevel != LogLevel.None;
        }

        private int GetCounterValue(IStorageConnection connection, string jobId, bool plus = false, TimeSpan? jobExpirationTimeout = null) 
        {
            string counterName = Util.GetCounterName(jobId);
            var counterHash = connection.GetAllEntriesFromHash(counterName);
            int counterValue = counterHash != null && counterHash.Any() ?
                int.Parse(counterHash.FirstOrDefault().Value) : 0;

            if (plus) 
            {
                using (var writeTransaction = connection.CreateWriteTransaction())
                {
                    lock (_lockObj) 
                    {
                        var dictionaryLogCounter = new Dictionary<string, string>
                        {
                            [counterName] = Convert.ToString(++counterValue)
                        };

                        writeTransaction.SetRangeInHash(counterName, dictionaryLogCounter);

                        if (writeTransaction is JobStorageTransaction jsTransaction)
                            jsTransaction.ExpireHash(counterName, jobExpirationTimeout ?? TimeSpan.MinValue);

                        writeTransaction.Commit();
                    }
                }
            }

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

                string counterName = Util.GetCounterName(jobId);
                int counterValue = GetCounterValue(connection, jobId, true, jobExpirationTimeout);

                var keyName = Util.GetKeyName(counterValue, jobId);
                var logSerialization = SerializationHelper.Serialize(logMessageModel);

                var dictionaryLog = new Dictionary<string, string>
                {
                    [keyName] = logSerialization
                };

                writeTransaction.SetRangeInHash(keyName, dictionaryLog);

                if (writeTransaction is JobStorageTransaction jsTransaction)
                {
                    jsTransaction.ExpireHash(keyName, jobExpirationTimeout);
                }

                writeTransaction.Commit();
            }
        }

        public IEnumerable<LogMessage> GetLogMessagesByJobId(IStorageConnection connection, string jobId, int from = 1, int count = 10)
        {
            var logMessages = new List<LogMessage>();

            try
            {
                int counterValue = GetCounterValue(connection, jobId);
                int toValue = count > counterValue ? counterValue : count;

                foreach (int i in Enumerable.Range(from, toValue)) 
                {
                    var logMessageHash = connection.GetAllEntriesFromHash(Util.GetKeyName(i, jobId));

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
