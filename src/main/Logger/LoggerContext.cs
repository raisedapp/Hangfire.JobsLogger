using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger.Logger
{
    internal class LoggerContext : ILogger
    {
        public static LoggerContext FromPerformContext(PerformContext context)
        {
            return context?.Items["LoggerContext"] as LoggerContext ?? null;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }


            
        }
    }
}
