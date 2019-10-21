using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobsLogger
{
    public class JobsLoggerOptions
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
    }
}