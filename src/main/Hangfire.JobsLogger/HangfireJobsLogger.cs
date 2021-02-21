using Hangfire.JobsLogger.Helper;
using Hangfire.JobsLogger.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Hangfire.JobsLogger
{
    public class HangfireJobsLogger : ILogger
    {
        private static readonly Logging.ILog HangfireInternalLog = Logging.LogProvider.For<HangfireJobsLogger>();

        public static void Log(string jobId, LogLevel logLevel, string logMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jobId))
                    return;

                var item = Util.GetLoggerContextName(jobId);

                if (JobsLoggerFilter.Loggers[item] is LoggerContext loggerContext &&
                    loggerContext.IsEnabled(logLevel))
                {
                    var context = loggerContext.PfContext;

                    using (var connection = context.Storage.GetConnection())
                    {
                        var jobExpirationTimeout = context.Storage.JobExpirationTimeout;

                        loggerContext.SaveLogMessage(connection, jobId, jobExpirationTimeout, logLevel, logMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var logLine = $"Error Write Log. Exception Message = {ex.Message}, StackTrace = {ex}";

                HangfireInternalLog.Log(Logging.LogLevel.Error, () => logLine);
                Trace.WriteLine(logLine);
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string jobId = string.Empty;

            string message = formatter(state, exception);

            Log(jobId, logLevel, message);
        }
    }
}
