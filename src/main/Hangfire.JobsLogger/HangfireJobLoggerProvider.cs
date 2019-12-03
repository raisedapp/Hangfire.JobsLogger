using Hangfire.JobsLogger.Helper;
using Hangfire.JobsLogger.Server;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hangfire.JobsLogger
{
    public class HangfireJobLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();

        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new HangfireJobsLogger());

        public void Dispose() => _loggers.Clear();
    }
}
