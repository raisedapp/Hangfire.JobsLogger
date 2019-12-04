using System;
using Microsoft.Extensions.Logging;

namespace Hangfire.JobsLogger
{
    public static class HangfireJobLoggerExtensions
    {
        public static void LogDebug(this ILogger logger, string jobId, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, jobId, eventId, exception, message, args);
        }

        public static void LogDebug(this ILogger logger, string jobId, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, jobId, eventId, message, args);
        }

        public static void LogDebug(this ILogger logger, string jobId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, jobId, exception, message, args);
        }

        public static void LogDebug(this ILogger logger, string jobId, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, jobId, message, args);
        }
        
        public static void LogTrace(this ILogger logger, string jobId, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, jobId, eventId, exception, message, args);
        }

        public static void LogTrace(this ILogger logger, string jobId, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, jobId, eventId, message, args);
        }

        public static void LogTrace(this ILogger logger, string jobId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, jobId, exception, message, args);
        }

        public static void LogTrace(this ILogger logger, string jobId, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, jobId, message, args);
        }


        public static void LogInformation(this ILogger logger, string jobId, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Information, jobId, eventId, exception, message, args);
        }

        public static void LogInformation(this ILogger logger, string jobId, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Information, jobId, eventId, message, args);
        }

        public static void LogInformation(this ILogger logger, string jobId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Information, jobId, exception, message, args);
        }

        public static void LogInformation(this ILogger logger, string jobId, string message, params object[] args)
        {
            logger.Log(LogLevel.Information, jobId, message, args);
        }
        
        public static void LogWarning(this ILogger logger, string jobId, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, jobId, eventId, exception, message, args);
        }

        public static void LogWarning(this ILogger logger, string jobId, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, jobId, eventId, message, args);
        }

        public static void LogWarning(this ILogger logger, string jobId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, jobId, exception, message, args);
        }

        public static void LogWarning(this ILogger logger, string jobId, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, jobId, message, args);
        }
        
        public static void LogError(this ILogger logger, string jobId, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, jobId, eventId, exception, message, args);
        }

        public static void LogError(this ILogger logger, string jobId, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, jobId, eventId, message, args);
        }

        public static void LogError(this ILogger logger, string jobId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, jobId, exception, message, args);
        }

        public static void LogError(this ILogger logger, string jobId, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, jobId, message, args);
        }


        public static void LogCritical(this ILogger logger, string jobId, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Critical, jobId, eventId, exception, message, args);
        }

        public static void LogCritical(this ILogger logger, string jobId, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Critical, jobId, eventId, message, args);
        }

        public static void LogCritical(this ILogger logger, string jobId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Critical, jobId, exception, message, args);
        }

        public static void LogCritical(this ILogger logger, string jobId, string message, params object[] args)
        {
            logger.Log(LogLevel.Critical, jobId, message, args);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string jobId, string message, params object[] args)
        {
            logger.Log(logLevel, jobId,0, null, message, args);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string jobId,  EventId eventId, string message, params object[] args)
        {
            logger.Log(logLevel, jobId, eventId, null, message, args);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string jobId, Exception exception, string message, params object[] args)
        {
            logger.Log(logLevel, jobId, 0, exception, message, args);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, string jobId, EventId eventId, Exception exception, string message, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            
            HangfireJobsLogger.Log(jobId, logLevel, message);
            logger.Log(logLevel, eventId, exception, message, args);
        }
    }
}