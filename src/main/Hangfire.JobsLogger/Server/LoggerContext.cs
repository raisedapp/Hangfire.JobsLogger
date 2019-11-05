using Hangfire.Common;
using Hangfire.JobsLogger.Model;
using Hangfire.Server;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    }
}
