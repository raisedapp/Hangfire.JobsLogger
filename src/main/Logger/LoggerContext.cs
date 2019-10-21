using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger.Logger
{
    internal class LoggerContext : ILogger
    {
        private static PerformContext _context;

        public static LoggerContext FromPerformContext(PerformContext context)
        {
            _context = context;
            return _context?.Items["LoggerContext"] as LoggerContext ?? null;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            LogMessage logMessage = new LogMessage() 
            {
                JobId = string.Empty,
                LogLevel = logLevel,
                DateCreation = DateTime.UtcNow,
                Message = string.Empty
            };

            //TODO: Write Log in Hangfire Storage
        }
    }
}
